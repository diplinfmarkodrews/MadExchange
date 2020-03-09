using Convey.CQRS.Events;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class CancelOrderEvent : IEvent
    {
        public Guid Id { get; }
        public CancelOrder Command { get; }        
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public CancelOrderEvent(CancelOrder command)
        {
            Id = command?.Id ?? Guid.NewGuid();
            Command = command;
            
        }
    }
}