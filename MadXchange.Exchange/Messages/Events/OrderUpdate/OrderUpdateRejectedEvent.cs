using Convey.CQRS.Events;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class OrderUpdateRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public UpdateOrder Command { get; }
        public Order Order { get; }
        public Guid Id { get; }
        public string Reason { get; }
        public string Code { get; }

        public OrderUpdateRejectedEvent(UpdateOrder command, Order order)
        {
            Id = order?.Id ?? Guid.NewGuid();
            Command = command;
            Order = order;
        }
    }
}