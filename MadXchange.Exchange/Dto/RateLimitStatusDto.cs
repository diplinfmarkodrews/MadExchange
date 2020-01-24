using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto
{
    public class RateLimitStatusDto
    {
        public Guid AccountId { get; }
        public long TSNow { get; } = DateTime.UtcNow.Ticks;
        public int RateLimit { get; }
        public int RateLimitStatus { get; set; }

    }
}
