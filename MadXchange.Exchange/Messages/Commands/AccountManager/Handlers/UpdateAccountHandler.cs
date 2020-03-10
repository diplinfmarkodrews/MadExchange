using Convey.CQRS.Commands;
using MadXchange.Connector.Messages.Commands.AccountManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.AccountManager.Handlers
{
    public class UpdateAccountHandler : ICommandHandler<UpdateAccount>
    {
        public Task HandleAsync(UpdateAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
