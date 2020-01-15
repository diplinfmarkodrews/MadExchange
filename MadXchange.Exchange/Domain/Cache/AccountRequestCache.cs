using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Domain.Cache
{
    public class AccountRequestCache : RedisCache
    {

        public AccountRequestCache(IOptions<RedisCacheOptions> options) : base(options)
        {   
            
        }

        public new void Dispose() 
        {
            base.Dispose();
        }
    }
}
