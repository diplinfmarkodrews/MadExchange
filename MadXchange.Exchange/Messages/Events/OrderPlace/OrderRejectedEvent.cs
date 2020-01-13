using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class OrderRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public IOrder Order { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public OrderRejectedEvent(CreateOrder command, IOrder order)
        {
            Command = command;
            Order = order;
        }

        

    }
}
