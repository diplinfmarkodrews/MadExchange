using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class OrderRejectedEvent : IRejectedEvent
    {
        public Guid Id { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
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