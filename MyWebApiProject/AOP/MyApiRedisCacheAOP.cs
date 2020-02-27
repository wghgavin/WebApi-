using Castle.DynamicProxy;
using MyWebApiProject.Common;
using MyWebApiProject.Common.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.AOP
{
    public class MyApiRedisCacheAOP : CacheAOPbase
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly IRedisCacheManager _cache;
        public MyApiRedisCacheAOP(IRedisCacheManager cache)
        {
            _cache = cache;
        }
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            var qCachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute))
                as CachingAttribute;
            //拦截
            if (qCachingAttribute != null)
            {
                //获取自定义缓存键，和Memory内存缓存一样
                var cacheKey = CustomCacheKey(invocation);
                //获取值(string类型)核心一
                string cacheValue = _cache.GetValue(cacheKey);
                if (cacheValue != null)
                {
                   //获取返回类型
                    var type = invocation.Method.ReturnType;
                    if (type.FullName == "System.Void") return;//如果是同步并且是voi类型，返回
                    var resultTypes = type.GenericTypeArguments;//拿到泛型参数                  
                    object response;
                    if (type != null && typeof(Task).IsAssignableFrom(type))//异步
                    {
                        //返回异步的对象Task<T>核心二
                        if (resultTypes.Count() > 0)//泛型的参数存在
                        {                           
                            var resultType = resultTypes.FirstOrDefault();
                            dynamic temp = Newtonsoft.Json.JsonConvert.DeserializeObject(cacheValue, resultType);//把自定义key转为泛型参数同一个类型
                            response = Task.FromResult(temp);//然后放入response
                        }
                        else
                        {
                            //Task无返回方法,指定时间不允许重新运行
                            response = Task.Yield();
                        }
                    }
                    else//如果是同步的，把获取的值转为返回值类型即可
                    {
                       //核心4，要进行ChangeType
                        response = System.Convert.ChangeType(_cache.Get<object>(cacheKey), type);
                    }
                    invocation.ReturnValue = response;
                    return ;
                }
                invocation.Proceed();//执行方法
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    object response;
                    var type = invocation.Method.ReturnType;
                    if (type != null && typeof(Task).IsAssignableFrom(type))
                    {
                       //异步拿到Task里面的result的方法
                        var resultProperty = type.GetProperty("Result");
                        response = resultProperty.GetValue(invocation.ReturnValue);
                    }
                    else
                    {
                        response = invocation.ReturnValue;
                    }
                    if (response == null) response = string.Empty;//如果返回值是空
                    //核心5:将获取到指定的response 和特性的缓存时间，进行set操作
                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(qCachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                //直接执行被拦截的方法
                invocation.Proceed();
            }
            
        }
    }
}
