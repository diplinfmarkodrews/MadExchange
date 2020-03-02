using ServiceStack;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MadXchange.Exchange.Contracts
{
    //[DataContract]
    //public class SocketRequestDto : IReturn<SocketMessageDto>
    //{
    //    [DataMember]
    //    public virtual SocketMethod Op { get; set; }
    //    [DataMember]
    //    public virtual string[] Args { get; set; }

    //    public SocketRequestDto(SocketMethod method, string[] args)
    //    {
    //        Op = method;
    //        Args = args;
    //    }
    //}
    [DataContract]
    public enum MessageType
    {
        Unknown = 0,
        [DataMember]
        Ctrl = 1,
        [DataMember]
        Data = 2
    }
    [DataContract]
    public enum DataType
    {
        Unspecified = 0,
        [DataMember]
        Snapshot = 1,
        [DataMember]
        Update = 2,
        [DataMember]
        Delta = 3,
        [DataMember]
        Partial = 4

    }

    public interface ISocketMessage
    {
        public MessageType MessageType { get; }
        
    }

    [DataContract]
    public class SocketResponse : ISocketMessage
    {
        public MessageType MessageType { get; } = MessageType.Ctrl;
        public SocketMethod Method { get; private set; }
        
        [DataMember]
        public virtual ObjectDictionary Response { get; set; }
        private bool isSuccess { get; set; }
        public virtual bool IsSuccess() => isSuccess;
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        internal string GetTopic(string topicString) => Response.GetValueOrDefault(topicString).ToString();
        
    }

    //[DataContract]
    //public class SocketMessage : ISocketMessage
    //{
        
    //    public MessageType MessageType { get; } = MessageType.Data;        
    //    public DataType DataType { get; set; } 
        
    //    [DataMember]
    //    public string Data { get; set; }
        
    //    public DateTime Timestamp { get; } = DateTime.UtcNow;

    //    //internal virtual string GetChannel(string channelString)
    //    //    => Data.GetValueOrDefault(channelString).ToString();

    //}



    public interface ISocketRequest : IReturnVoid, IDisposable 
    {
        SocketMethod Method { get; }
    }

    [DataContract]
    public class SocketRequest : ISocketRequest 
    {
        
        public SocketMethod Method { get; set; }
        [DataMember]
        public ObjectDictionary Parameter { get;  }
        
        public SocketRequest(SocketMethod method, string[] parameter)
        {
            Parameter = new ObjectDictionary() { { "op", method.ToString().ToLower() }, { "args", parameter } };            
            Method = method;
        }

        public string ToSocketRequestDto() 
        {
            Timestamp = DateTime.UtcNow;
            RequestDto = Parameter.ToJson();
            return RequestDto;
        }

        public string RequestDto { get; private set; }
        public DateTime Timestamp { get; private set; } 
       

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    RequestDto = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                Parameter.Clear();
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SocketRequest()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
    
}
