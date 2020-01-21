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
        Guid AccountId { get; }
        public Guid ExchangeId { get; }
        public string Subscription { get; }
    }
}