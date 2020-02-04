using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Contracts;
using System;

namespace MadXchange.Connector.Interfaces
{
    public interface IClientSocket
    {
        
        bool IsConnected(Guid accountId);
        bool Connect(Guid accountId);
        bool SubScribeSocket(ISubscrition subscription);        
        void Unsubscribe(ISubscrition subscription);
    }

    public interface ISubscrition
    {

        Guid AccountId { get; }
        public Exchanges Exchange { get; }
        public string Subscription { get; }
    }
}