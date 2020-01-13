using MadXchange.Common.Messages;
using System.Threading.Tasks;

namespace MadXchange.Common.Handlers
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task HandleAsync(TCommand @command);
    }
}
