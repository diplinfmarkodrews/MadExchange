using MadXchange.Connector.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Exceptions
{
    public sealed class SocketRequestException : Exception
    {
        public Guid WebSocketConnectionId { get; }
        public SocketRequestException(Guid connectionId, string message) : base(message) 
        {
            WebSocketConnectionId = connectionId;
        }
    }
}
