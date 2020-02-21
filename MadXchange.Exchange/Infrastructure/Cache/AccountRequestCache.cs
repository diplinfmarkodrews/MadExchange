using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Interfaces.Cache;
using Microsoft.Extensions.Caching.Distributed;
using ServiceStack.Redis;
using System;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class AccountRequestCache : CacheStorage<AccountCacheObject>, IAccountRequestCache
    {
        public AccountRequestCache(IRedisClientsManager cache) : base("request", cache)
        {
        }

        public void SetAccount(AccountCacheObject account)
        {
            Set($"{account.AccountId}", account);
        }

        public AccountCacheObject GetAccount(Guid accountId)
        {
            return Get($"{accountId}");
        }
    }
}