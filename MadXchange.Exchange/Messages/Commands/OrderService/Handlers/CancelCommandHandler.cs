using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.OrderService.Handlers
{
    public class CancelCommandHandler : ICommandHandler<CancelCommand>
    {
        public Task HandleAsync(CancelCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
