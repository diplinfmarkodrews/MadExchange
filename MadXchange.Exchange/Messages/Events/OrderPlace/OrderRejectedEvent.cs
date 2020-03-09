using Convey.CQRS.Events;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class OrderRejectedEvent : IRejectedEvent
    {
        public Guid Id { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public CreateOrder Command { get; }
        public Order Order { get; }
        public string Reason { get; }
        public string Code { get; }

        public OrderRejectedEvent(CreateOrder command, Order order)
        {
            Id = order?.Id ?? Guid.NewGuid();
            Command = command;
            Order = order;
        }
    }
}