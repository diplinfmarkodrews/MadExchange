using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Text;

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
