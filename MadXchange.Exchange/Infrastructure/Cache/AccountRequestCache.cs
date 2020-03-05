using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    
    public interface IAccountRequestCache
    {
        void SetAccount(AccountRequestCacheObject account);
        AccountRequestCacheObject GetAccount(Guid accountId);
        Task<IDisposable> LockAccount(Guid accountId);
    }

    public class AccountRequestCache : CacheStorageTransient<AccountRequestCacheObject>, IAccountRequestCache
    {
        public AccountRequestCache(IRedisClientsManager cache) : base("request", cache) { }
       
        public void SetAccount(AccountRequestCacheObject account)
            => Set($"{account.AccountId}", account);

        public AccountRequestCacheObject GetAccount(Guid accountId)
            => Get($"{accountId}") ?? new AccountRequestCacheObject(accountId) { NextRequestTime = DateTime.UtcNow.Ticks };

        public Task<IDisposable> LockAccount(Guid accountId)        
            => Task.FromResult(AquireLock($"{accountId}"));
        
    }
}