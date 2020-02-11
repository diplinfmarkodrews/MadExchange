using Convey.CQRS.Events;
using System;

namespace MadXchange.Exchange.Handlers.Equity
{
    public class EquityRequestRejectedEvent : IRejectedEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime TimeStamp { get; } = DateTime.UtcNow;
        public string Reason { get; }
        public string Code { get; }
    }
}