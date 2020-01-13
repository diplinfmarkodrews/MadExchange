using MadXchange.Exchange.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MadXchange.Exchange.Infrastructure
{
    public interface IClientStore /// implementation in CQRS => Queries
    {

        Task<IMargin> GetMarginAsync(string cur);
        Task<IPosition> GetPositionAsync(string symbol);
        Task<List<IOrder>> GetOrdersAsync(string symbol);
    }             
}
