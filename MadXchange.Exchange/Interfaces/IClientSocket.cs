using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
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