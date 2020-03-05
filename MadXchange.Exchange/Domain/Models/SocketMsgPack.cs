using MadXchange.Exchange.Domain.Types;
using System;

namespace MadXchange.Connector.Domain.Models
{
    
    public interface IDataPack 
    {
    
    }
    
    public sealed class SocketMsgPack : IDataPack
    {
        public Guid Id { get; }
        public Xchange Exchange { get; }
        public object Data { get; }
        public long Timestamp { get; set; }

        public SocketMsgPack(Guid id, Xchange xchange, object data, long timestamp)
        {
            Id = id;
            Exchange = xchange;
            Data = data;
            Timestamp = timestamp;
        }
    }


    public sealed class SocketUpdatePack : IDataPack
    {
        public Guid Id { get; }
        public Xchange Exchange { get; }
        public object Insert { get; }
        public object Update { get; }
        public object Delete { get; }
        public long Timestamp { get; set; }

        public SocketUpdatePack(Guid id, Xchange xchange, object insert, object update, object delete, long timestamp)
        {
            Id = id;
            Exchange = xchange;
            Insert = insert;
            Update = update;
            Delete = delete;
            Timestamp = timestamp;
        }
    }
}