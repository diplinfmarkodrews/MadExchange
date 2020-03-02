using Convey.CQRS.Commands;
using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;

namespace MadXchange.ClientExecution.Services
{
    public class ClientRegisterSocket : ICommand
    {
        public Guid CommandId { get; set; }
        public Guid AccountId { get; set; }
        public IEnumerable<SocketSubscription> Subscriptions { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

    }
}