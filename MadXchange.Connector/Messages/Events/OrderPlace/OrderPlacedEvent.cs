using Convey.CQRS.Events;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class OrderPlacedEvent : IEvent
    {
        public Guid Id { get; }
        public CreateOrder Command { get; }
        public Order Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public OrderPlacedEvent(CreateOrder command, Order order) 
        {
            Id = order.Id;
            Order = order;
            Command = command;
        }
    }
}
