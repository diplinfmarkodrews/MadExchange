using MadXchange.Common.Messages;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System;


namespace MadXchange.Connector.Messages.Events
{
    public class OrderUpdateRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public IOrder Order { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public OrderUpdateRejectedEvent(UpdateOrder command, IOrder order)
        {
            Command = command;
            Order = order;
        }

    
    }
}
