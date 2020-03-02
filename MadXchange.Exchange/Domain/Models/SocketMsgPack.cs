using MadXchange.Exchange.Domain.Types;
using System;

namespace MadXchange.Connector.Domain.Models
{
    public sealed class SocketMsgPack
    {
        public Guid Id { get; }
        public Xchange Exchange { get; }
        public object Data { get; }
        public long Timestamp { get; set; }

        public SocketMsgPack(Guid id, Xchange xchange, object result, long ts)
        {
            Id = id;
            Exchange = xchange;
            Data = result;
            Timestamp = ts;
        }
    }
}