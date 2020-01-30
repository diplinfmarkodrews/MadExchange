using MadXchange.Exchange.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{

    public interface IAccountManager 
    {
    
    }

    public class AccountManager : IAccountManager
    {
        private readonly IApiKeySetRepository _apiKeySetRepository;
        private readonly ISocketConnectionService _socketConnectionService;
        private readonly ILogger _logger;
        public AccountManager(IApiKeySetRepository apikeysetRepo, ISocketConnectionService socketConnectionService, ILogger<AccountManager> logger) 
        {
            _apiKeySetRepository = apikeysetRepo;
            _socketConnectionService = socketConnectionService;
            _logger = logger;
        }

    }
}
