using Convey.CQRS.Commands;
using System;
using System.Threading;

namespace MadXchange.Exchange.Domain.Models
{
    public class CommandItem
    {
        public Guid Id { get; }
        public ICommand Command { get; set; }
        public CancellationTokenSource CancellationSource { get; set; }
        public CommandItem(Guid id, ICommand cmd) 
        {
            Id = id;
            Command = cmd;
            CancellationSource = new CancellationTokenSource();
        }
    }
}