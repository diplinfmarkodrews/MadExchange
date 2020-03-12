using Convey.CQRS.Commands;
using System;
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
