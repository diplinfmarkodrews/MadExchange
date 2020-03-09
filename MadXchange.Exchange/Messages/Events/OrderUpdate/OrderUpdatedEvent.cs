using Convey.CQRS.Events;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class OrderUpdatedEvent : IEvent
    {
        public Guid Id { get; }
        public UpdateOrder Command { get; }
        public Order Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;

        public OrderUpdatedEvent(UpdateOrder command, Order order)
        {
            Id = order?.Id ?? Guid.NewGuid();
            Command = command;
            Order = order;
        }
    }
}