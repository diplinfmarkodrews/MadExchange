using Convey.CQRS.Events;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class CancelOrderRejectedEvent : IRejectedEvent
    {
        public Guid Id { get; }
        public CancelOrder Command { get; }        
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public string Reason { get; }
        public string Code { get; }

        public CancelOrderRejectedEvent(CancelOrder command)
        {
            Id = command?.Id ?? Guid.NewGuid();
            Command = command;
            
        }
    }
}