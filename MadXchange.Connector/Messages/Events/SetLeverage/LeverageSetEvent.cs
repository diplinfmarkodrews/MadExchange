using Convey.CQRS.Commands;
using Convey.CQRS.Events;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Connector.Messages.Events
{
    public class LeverageSetEvent : IEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public IPosition Position { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public LeverageSetEvent(Commands.SetLeverage command, IPosition position)
        {
            Command = command;
            Position = position;
        }
    }
}