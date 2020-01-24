using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Domain.Cache
{
    public class AccountCacheObject
    {
        public Guid AccountId { get; }
        public int RateLimitStatus { get; set; }
        public int LastRateLimit { get; set; }
        public int RequestQueue { get; set; }
        //Given in Ticks
        public long LastRequestTime { get; set; }
        //Given in Ticks
        public long NextRequestTime { get; set; }
        

    }
}
