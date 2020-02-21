using System;

namespace MadXchange.Exchange.Domain.Cache
{
    [Serializable]
    public class AccountCacheObject : ICacheObject
    {
        public Guid AccountId { get; }
        public int RateLimitStatus { get; set; }
        public int LastRateLimit { get; set; }
        public int RequestQueue { get; set; }

        //Given in Ticks
        public long LastRequestTime { get; set; }

        //Given in Ticks
        public long NextRequestTime { get; set; }

        //Given in Ticks
        public long Timestamp { get; set; }

        public AccountCacheObject(Guid accountId)
        {
            AccountId = accountId;
        }

        public bool IsValid() =>
              Timestamp == default
           || LastRequestTime == default
           || NextRequestTime == default
           ? false
           : true;
    }
}