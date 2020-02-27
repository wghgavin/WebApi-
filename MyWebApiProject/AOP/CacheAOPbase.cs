using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.AOP
{
    public abstract class CacheAOPbase: IInterceptor
    {
        public abstract void Intercept(IInvocation invocation);
        //自定义缓存键
        protected string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;//方法类型名称
            var methodName = invocation.Method.Name;//方法的名称
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();
            //获取参数列表，最多需要三个即可
            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key += $"{param}:";
            }
            return key.TrimEnd(':');
        }
        //object 转 string
        protected string GetArgumentValue(object arg)
        {
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return "";
        }
    }
}
