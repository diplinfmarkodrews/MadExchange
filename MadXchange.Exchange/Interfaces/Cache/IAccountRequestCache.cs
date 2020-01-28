using MadXchange.Exchange.Domain.Cache;
using System;

namespace MadXchange.Exchange.Interfaces.Cache
{
    public interface IAccountRequestCache
    {
        void SetAccount(AccountCacheObject account);
        AccountCacheObject GetAccount(Guid accountId);        
    }
}