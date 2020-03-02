using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Stores;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MadXchange.Connector.Services
{
    public interface IAccountManager
    {
        void RegisterClients();
        void DeRegisterClients();
    }

    /// <summary>
    /// To:
    /// - signal the key-store to add or remove accounts
    /// - signal the SocketConnectionService which connections to open and close
    /// </summary>
    public class AccountManager : IAccountManager
    {
        private readonly IApiKeySetStore _apiKeySetRepository;
        private readonly ISocketConnectionService _socketConnectionService;
        private readonly ILogger _logger;

        public AccountManager(IApiKeySetStore apikeysetRepo, ISocketConnectionService socketConnectionService, ILogger<AccountManager> logger)
        {
            _apiKeySetRepository = apikeysetRepo;
            _socketConnectionService = socketConnectionService;
            _logger = logger;
        }

        public void DeRegisterClients()
        {
            throw new NotImplementedException();
        }

        public void RegisterClients() 
        {
            var subscriptions = new List<SocketSubscription>();
            var btcSubscription = new SocketSubscription(Guid.NewGuid(), "Instrument", "instrument_info", new List<string>() { "100ms", "BTCUSD" });
            subscriptions.Add(btcSubscription);
            _socketConnectionService.RegisterSocketInstrument(Exchange.Domain.Types.Xchange.ByBit, subscriptions, Guid.NewGuid());
            
        }
    }
}