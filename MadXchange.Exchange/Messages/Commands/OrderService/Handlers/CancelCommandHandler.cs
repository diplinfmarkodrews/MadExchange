using Convey.CQRS.Commands;
using Convey.MessageBrokers;
using MadXchange.Exchange.Infrastructure.Stores;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Messages.Commands.OrderService.Handlers
{
    public class CancelCommandHandler : ICommandHandler<CancelCommand>
    {
        private readonly ICommandStore _commandStore;
        private readonly IBusPublisher _busPublisher;

        public CancelCommandHandler(ICommandStore commandStore, IBusPublisher busPublisher)
        {
            _busPublisher = busPublisher;
            _commandStore = commandStore;
        }
        public Task HandleAsync(CancelCommand command)
        {
            var cmd = _commandStore.GetCommand(command.CmdId);
            cmd.CancellationSource.Cancel();
            return Task.CompletedTask;
        }
    }
}
