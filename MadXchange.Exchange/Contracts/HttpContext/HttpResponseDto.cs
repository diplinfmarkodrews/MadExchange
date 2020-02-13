using System;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts.Http
{
    [DataContract]
    public class HttpResponseDto
    {
        [DataMember]
        public virtual string RetMsg { get; set; }

        [DataMember]
        public virtual string Result { get; set; }

        [DataMember]
        public virtual int RetCode { get; set; }

        [DataMember]
        public virtual int ExtCode { get; set; }

        [DataMember]
        public virtual string ExtInfo { get; set; }

        [DataMember]
        public virtual string TimeNow { get; set; }

        [DataMember]
        public virtual int RateLimit { get; set; }

        [DataMember]
        public virtual int RateLimitStatus { get; set; }

        [DataMember]
        public virtual long RateLimitReset { get; set; }

        public long Timestamp { get; } = DateTime.UtcNow.Ticks;
    }
}