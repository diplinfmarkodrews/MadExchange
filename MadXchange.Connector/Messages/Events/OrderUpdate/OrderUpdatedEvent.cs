using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System;


namespace MadXchange.Connector.Messages.Events
{

    public class OrderUpdatedEvent  : IEvent
    {
        public Guid Id { get; } 
        public ICommand Command { get; }
        public IOrder Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public OrderUpdatedEvent(UpdateOrder command, IOrder order) 
        {
            Id = order?.Id ?? Guid.NewGuid();
            Command = command;
            Order = order;
        }
    }
}
