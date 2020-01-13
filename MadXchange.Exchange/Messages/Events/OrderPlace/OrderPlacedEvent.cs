using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class OrderPlacedEvent : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public ICommand Command { get; }
        public IOrder Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public OrderPlacedEvent(CreateOrder command, IOrder order) 
        {
            Order = order;
            Command = command;
        }
    }
}
