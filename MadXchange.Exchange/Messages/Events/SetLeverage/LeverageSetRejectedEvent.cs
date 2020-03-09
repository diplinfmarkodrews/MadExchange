using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class LeverageSetRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public IPosition Position { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public string Reason { get; }

        public string Code { get; }

        public LeverageSetRejectedEvent(SetLeverage command, IPosition position)
        {
            Command = command;
            Position = position;
        }
    }
}