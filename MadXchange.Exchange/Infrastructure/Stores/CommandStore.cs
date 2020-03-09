using Convey.CQRS.Commands;
using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    public interface ICommandStore
    {
        CommandItem GetCommand(Guid commandId);
        CancellationToken RegisterCommand(Guid id, ICommand cmd);
        void ClearCommand(Guid accountId);
    }

    public class CommandStore : ICommandStore
    {

        private Dictionary<Guid, CommandItem> _commandStoreDic = new Dictionary<Guid, CommandItem>();

        public void ClearCommand(Guid accountId)
            => _commandStoreDic.Remove(accountId);
        

        public CommandItem GetCommand(Guid commandId)
            =>  _commandStoreDic.GetValueOrDefault(commandId);
        
        //we need an identifiabl command
        public CancellationToken RegisterCommand(Guid id, ICommand cmd)
        {
            var result = new CommandItem(id, cmd);
            _commandStoreDic[id] = result;
            return result.CancellationSource.Token;
        }
    }
}
