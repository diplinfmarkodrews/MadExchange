using MadXchange.Common.Messages;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class OrderUpdatedEvent  : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public ICommand Command { get; }
        public IOrder Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public OrderUpdatedEvent(UpdateOrder command, IOrder order) 
        {
            Command = command;
            Order = order;
        }
    }
}
