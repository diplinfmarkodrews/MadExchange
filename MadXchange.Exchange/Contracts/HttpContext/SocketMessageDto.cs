using System;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{

    [DataContract]
    public class SocketMessageDto
    {   //we ask first if data field is filled to optimize for data access
        public MessageType MsgType => Data != null ? MessageType.Data : Success != null ? MessageType.Ctrl : MessageType.Unknown;
        [DataMember]
        public virtual bool? Success { get; set; }
        [DataMember]
        public virtual string RetMsg { get; set; }
        [DataMember]
        public string Request { get; set; }
        [DataMember]
        public virtual string Topic { get; set; }
        [DataMember]
        public virtual string Action { get; set; }
        [DataMember]
        public virtual string Type { get; set; }
        [DataMember]
        public virtual string Data { get; set; }
        [DataMember(Name = "ConnId")]
        public virtual string ConnectId { get; set; }
        public virtual long Timestamp { get; } = DateTime.UtcNow.Ticks;

    }

}