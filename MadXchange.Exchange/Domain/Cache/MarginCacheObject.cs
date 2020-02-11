using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Domain.Cache
{
    public class MarginCacheObject : ICacheObject
    {
        public Guid AccountId { get; }
        public Margin MarginObj { get; set; }

        public MarginCacheObject(Guid accountId)
        {
            AccountId = accountId;
        }

        public bool IsValid() =>
               MarginObj is null
            || MarginObj.Timestamp == default
            || MarginObj.Currency == default
            ? false
            : true;
    }
}