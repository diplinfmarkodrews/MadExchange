using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using ServiceStack.Redis;
using System;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public interface IAccountRequestCache
    {
        void SetAccount(AccountCacheObject account);
        AccountCacheObject GetAccount(Guid accountId);
        IDisposable LockAccount(Guid accountId);
    }
    public class AccountRequestCache : CacheStorageTransient<AccountCacheObject>, IAccountRequestCache
    {
        public AccountRequestCache(IRedisClientsManager cache) : base("request", cache) { }

        
        public void SetAccount(AccountCacheObject account)
            =>  Set($"{account.AccountId}", account);

        public AccountCacheObject GetAccount(Guid accountId)
            => Get($"{accountId}") ?? new AccountCacheObject(accountId) { NextRequestTime = DateTime.UtcNow.Ticks };

        public IDisposable LockAccount(Guid accountId)        
            => base.AquireLock($"{accountId}");
        
    }
}