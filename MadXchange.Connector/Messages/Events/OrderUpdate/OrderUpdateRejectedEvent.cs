using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System;


namespace MadXchange.Connector.Messages.Events
{
    public class OrderUpdateRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public IOrder Order { get; }
        public Guid Id { get; } = Guid.NewGuid();
        public string Reason { get; }
        public string Code { get; }

        public OrderUpdateRejectedEvent(UpdateOrder command, IOrder order)
        {
            Command = command;
            Order = order;
        }

    
    }
}
