using Convey.CQRS.Events;
using System;

namespace MadXchange.Exchange.Messages.Events.CancelCommand
{
    public class CancelCommandRejectedEvent : IRejectedEvent
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
        public string Code { get; set; }
    }
}
