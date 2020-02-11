using MadXchange.Exchange.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MadXchange.MadClient.Interface
{
    /// <summary>
    /// to access cached status variables, implemented as Infrastructure
    /// </summary>
    public interface IClientStore
    {
        Task<IMargin> GetMarginAsync(string cur);

        Task<IPosition> GetPositionAsync(string symbol);

        Task<List<IOrder>> GetOrdersAsync(string symbol);
    }
}