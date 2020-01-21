using MadXchange.Exchange.Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using MadXchange.Common.Helpers;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class AccountRequestCache : IDisposable
    {

        private readonly IDistributedCache _accountCache;
        public AccountRequestCache(IOptions<RedisCacheOptions> options) 
        {
            _accountCache = new RedisCache(options);
        }
        public async Task AddAccount(IUserAccount account) 
        {
            var accountByteArray = Converter.ObjectToByteArray(account);
            await _accountCache.SetAsync(account.Id.ToString(), accountByteArray);
        }
        
        public void Dispose() 
        {

            (_accountCache as RedisCache).Dispose();
        }

    }
}
