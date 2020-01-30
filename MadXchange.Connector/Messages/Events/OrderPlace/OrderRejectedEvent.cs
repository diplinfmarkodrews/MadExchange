using MadXchange.Common.Messages;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class OrderRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public OrderDto Order { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public OrderRejectedEvent(CreateOrder command, OrderDto order)
        {
            Command = command;
            Order = order;
        }

        

    }
}
