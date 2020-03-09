using Convey.CQRS.Commands;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Connector.Messages.Commands.AccountManager
{

    public class ClientRegisterSocket : ICommand
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public IEnumerable<SocketSubscription> Subscriptions { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

        
    }
    /// <summary>
    /// Fetching Account credentials from vault service, storing it in memory dictionary.
    /// setting all properties up for operation
    /// </summary>

    public class AddAccount : ICommand
    {
        public Guid Id { get; }
        public Guid AccountId { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public Xchange Exchange { get; set; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}