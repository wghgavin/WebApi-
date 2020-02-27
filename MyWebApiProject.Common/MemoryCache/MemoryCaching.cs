﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebApiProject.Common.MemoryCache
{
   public class MemoryCaching:ICaching
    {
        //引用Microsoft.Extensions.Caching.Memory;这个和.net 还是不一样，没有了Httpruntime了
        private IMemoryCache _cache;
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }
        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }
        public void Set(string cacheKey, object cacheValue,TimeSpan timeSpan)
        {
            _cache.Set(cacheKey, cacheValue, timeSpan);
        }
    }
}
