using Castle.DynamicProxy;
using MyWebApiProject.Common.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApiProject.AOP
{
    public class MyApiRedisCacheAOP
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly IRedisCacheManager _cache;
        public MyApiRedisCacheAOP(IRedisCacheManager cache)
        {
            _cache = cache;
        }
        public override void Intercept(IInvocation invocation)
        {

        }
    }
}
