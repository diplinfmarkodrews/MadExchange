using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IExchangeQueryClient
    {
        Task<IMargin> GetMargin(Guid accountId, string symbol = null);
        Task<IPosition> GetPosition(Guid accountId, string symbol = null);


    }
}
