using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public class AccountRequestCache : RedisCache
    {

        private readonly IDistributedCache _accountCache;
        public AccountRequestCache(IOptions<RedisCacheOptions> options) : base(options)
        {
            _accountCache = new RedisCache(options);
        }

        public new void Dispose() 
        {
            base.Dispose();
        }
    }
}
