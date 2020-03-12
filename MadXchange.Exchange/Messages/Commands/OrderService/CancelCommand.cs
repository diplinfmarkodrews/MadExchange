using Convey.CQRS.Commands;
using System;

namespace MadXchange.Exchange.Messages.Commands.OrderService
{
    public class CancelCommand : ICommand
    {
        public Guid CmdId { get; set; }
    }
}