using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class CancelOrderEvent : IEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public ICommand Command { get; }
        public IOrder Order { get; }
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public CancelOrderEvent(Commands.CancelOrder command, IOrder order) 
        {
            Command = command;
            Order = order;
        }
    }
}
