using MadXchange.Exchange.Configuration;
using System;
using System.Threading.Tasks;

namespace MadXchange.Connector.Repositories
{
    public interface IAccountRepository
    {
        Task<ExchangeDescriptor> GetAsync(Guid id);
        Task AddAsync(ExchangeDescriptor exchange);

        
    }
}
