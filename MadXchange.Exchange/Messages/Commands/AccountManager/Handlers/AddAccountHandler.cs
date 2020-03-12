using Convey.CQRS.Commands;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.AccountManager.Handlers
{
    public class AddAccountHandler : ICommandHandler<AddAccount>
    {
        public Task HandleAsync(AddAccount command)
        {
            throw new NotImplementedException();
        }
    }
}
