using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts.Http
{
    
    [DataContract(Name = "WebResponse")]
    public class WebResponseDto 
    {
        
        public Guid AccountId { get; set; }
        [DataMember]
        public string RetMsg { get; set; }
        [DataMember]
        public string Result { get; set; }
        [DataMember]
        public int RetCode { get; set; }
        [DataMember]
        public int ExtCode { get; set; }
        [DataMember]
        public string ExtInfo { get; set; }
        [DataMember]
        public string TimeNow { get; set; }
        [DataMember]
        public int RateLimit { get; set; }
        [DataMember]
        public int RateLimitStatus { get; set; }
        [DataMember]
        public long RateLimitReset { get; set; }
        public long Timestamp { get; } = DateTime.UtcNow.Ticks;
       

        
    }
}
