﻿using Castle.DynamicProxy;
using MyWebApiProject.Common;
using MyWebApiProject.Common.MemoryCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.AOP
{
    public class MyApiCacheAOP:CacheAOPbase
    {
        //通过注入把缓存操作的接口通过构造函数注入
        private ICaching _cache;
        public MyApiCacheAOP(ICaching cache)
        {
            _cache = cache;
        }
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            
            //对当前方法的特性验证
            if (method.GetCustomAttributes(true).FirstOrDefault(x=>x.GetType()==typeof(CachingAttribute)) is CachingAttribute cachingAttribute)
            {
                //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                //根据key获取相应的缓存值
                var cacheValue = _cache.Get(cacheKey);
                if(cacheValue != null)
                {
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //执行当前方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue,TimeSpan.FromMinutes(cachingAttribute.AbsoluteExpiration));
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }
       
    }
}
