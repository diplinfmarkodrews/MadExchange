using MadXchange.Common.Infrastructure;
using MadXchange.Exchange.Domain.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace MadXchange.Exchange.Infrastructure.Cache
{
    public class AccountRequestCache : CacheStorage<AccountCacheObject>
    {

        public AccountRequestCache(IDistributedCache cache) : base("request", cache) { }

        public void SetAccount(AccountCacheObject account)
        {
            
            Set($"{account.AccountId}", account); 
        }

        public AccountCacheObject GetAccountCacheObject(Guid accountId) 
        {
            var accountCacheObject = Get($"{accountId}");
            return accountCacheObject;            
        }                
    }
}
