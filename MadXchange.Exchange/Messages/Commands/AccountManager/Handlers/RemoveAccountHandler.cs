using Convey.CQRS.Commands;
using System;
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
