using MadXchange.Common.Messages;
using MadXchange.Exchange.Domain.Models;
using System;

namespace MadXchange.Exchange.Messages.Events.SetLeverage
{
    public class LeverageSetRejectedEvent : IRejectedEvent
    {
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public ICommand Command { get; }
        public IPosition Position { get; }
        public Guid Id { get; } = Guid.NewGuid();
        public LeverageSetRejectedEvent(Commands.SetLeverage command, IPosition position) 
        {
            Command = command;
            Position = position;
        }
    }
}
