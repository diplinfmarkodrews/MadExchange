using Convey.CQRS.Events;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Messages.Commands.OrderService;
using System;

namespace MadXchange.Exchange.Messages.Events
{
    public class LeverageSetEvent : IEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public SetLeverage Command { get; }
        public Position Position { get; }
        public Guid Id { get; } = Guid.NewGuid();

        public LeverageSetEvent(SetLeverage command, Position position)
        {
            Command = command;
            Position = position;
        }
    }
}