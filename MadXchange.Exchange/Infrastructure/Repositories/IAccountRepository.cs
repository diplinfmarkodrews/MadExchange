using MadXchange.Common.Types;
using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Repositories
{
    public interface IAccountRepository : IAsyncRepository<IUserAccount>
    {
        public Task<IUserAccount> GetAccountAsync(Guid accountId);
        public Task AddAccountAsync(IUserAccount account);
        public Task RemoveAccountAsync(Guid accountId);
    }
}
