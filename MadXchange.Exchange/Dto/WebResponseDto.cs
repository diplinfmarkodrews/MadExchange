using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto.Http
{
    public class WebResponseDto
    {
        public string Message { get; set; }
        public string Result { get; set; }
        public int Code { get; set; }
        public string ExtCode { get; set; }
        public string ExtInfo { get; set; }
        public string TimeNow { get; set; }
        public int RateLimit { get; set; }
        public int RateLimitStatus { get; set; }
        public long RateLimitReset { get; set; }
        public long Timestamp { get; } = DateTime.UtcNow.Ticks;
    }
}
