using Convey.CQRS.Commands;
using MadXchange.Connector.Messages.Commands.AccountManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.AccountManager.Handlers
{
    public class RemoveAccountHandler : ICommandHandler<RemoveAccount>
    {
        public Task HandleAsync(RemoveAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
