using ServiceStack;
using ServiceStack.Messaging;
using System.Runtime.Serialization;

namespace MadXchange.Exchange.Contracts
{

   
    [DataContract]
    public class SocketRequestDto : IReturn<SocketMessageDto>
    {
        [DataMember(Name = "op")]
        public virtual SocketMethod Operation { get; set; }
        [DataMember(Name = "args")]
        public virtual string Arguments { get; set; }
        public SocketRequestDto(SocketMethod method, string args) 
        {
            Operation = method;
            Arguments = args;
        }
    }

    public enum MessageType
    {
        Unknown = 0,
        Ctrl = 1,
        Data = 2
    }
    [DataContract]
    public class SocketMessageDto 
    {   
        public MessageType Type => !string.IsNullOrEmpty(Data) ? MessageType.Data : Request != null ? MessageType.Ctrl : MessageType.Unknown; 
        [DataMember]
        public virtual bool Success { get; set; }
        [DataMember]
        public virtual string RetMsg { get; set; }
        [DataMember]
        public SocketMessageDto Request { get; set; }
        [DataMember]
        public virtual string Topic { get; set; }
        [DataMember]
        public virtual string Action { get; set; }
        [DataMember]
        public virtual string Data { get; set; }

    }
}