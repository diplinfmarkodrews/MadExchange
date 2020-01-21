using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Domain.Cache
{
    public class AccountCacheObject
    {
        public Guid AccountId { get; }
        public int RateLimitStatus { get; set; }
        public DateTime LastRequestTime { get; set; }
        public long NextRequestTime { get; set; }

    }
}
