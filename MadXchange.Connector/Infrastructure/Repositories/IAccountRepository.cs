using MadXchange.Common.Types;
using MadXchange.Exchange.Configuration;
using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Connector.Infrastructure.Repositories
{
    public interface IAccountRepository : IAsyncRepository<IUserAccount>
    {
        Task<IUserAccount> GetAsync(Guid id);
        Task AddAsync(IUserAccount account);
        Task RemoveAccountAsync(Guid id);        
    }
}
