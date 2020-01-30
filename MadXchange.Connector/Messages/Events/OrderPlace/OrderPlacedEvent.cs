using MadXchange.Common.Messages;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class OrderPlacedEvent : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public CreateOrder Command { get; }
        public OrderDto Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public OrderPlacedEvent(CreateOrder command, OrderDto order) 
        {
            Order = order;
            Command = command;
        }
    }
}
