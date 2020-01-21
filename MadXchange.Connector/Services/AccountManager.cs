using MadXchange.Connector.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using StackExchange;
//using StackExchange.IO;
using Microsoft.Extensions.Caching.Memory;

namespace MadXchange.Connector.Services
{
    public interface IAccountManager 
    {
        Task<string> SignAccount(Guid accountId, string url); 
    }
    public class AccountManager : IAccountManager
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger _logger;
        private readonly IMemoryCache _requestCache;
        public AccountManager(IAccountRepository accountRepository,  ILogger<AccountManager> logger) 
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public Task<string> SignAccount(Guid accountId, string url)
        {
            //url.Auth
            return null;
        }
    }
}
