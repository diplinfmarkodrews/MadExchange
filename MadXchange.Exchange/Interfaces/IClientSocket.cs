using MadXchange.Common.Mex.Specification;
using System;

namespace MadXchange.Common.Mex.Interfaces
{
    public interface IClientSocket
    {
        
        bool IsConnected();
        bool Connect();
        bool SubScribeSocket(ISubscrition subscription, Action<SocketMessage> message);        
        void Unsubscribe(ISubscrition subscription);
    }

    public interface ISubscrition
    {
        Guid Account { get; }
        public string Exchange { get; }
        public string Subscription { get; }
    }
}