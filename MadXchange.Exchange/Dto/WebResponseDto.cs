using ServiceStack;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto.Http
{
    
    public class WebResponseDto
    {
       
        public string ret_msg { get; set; }
        public string result { get; set; }
        public int ret_code { get; set; }
        public string ext_code { get; set; }
        public string ext_info { get; set; }
        public string time_now { get; set; }
        public int rate_limit { get; set; }
        public int rate_limit_status { get; set; }
        public long rate_limit_reset { get; set; }
        public long Timestamp { get; } = DateTime.UtcNow.Ticks;
    }
}
