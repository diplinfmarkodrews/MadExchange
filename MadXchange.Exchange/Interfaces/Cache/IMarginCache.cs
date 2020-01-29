using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IMarginCache
    {
        void Set(Guid accountId, string symbol, Margin item);
        long Update(Guid accountId, string symbol, Margin item);
        Task<Margin> GetAsync(Guid accountId, string symbol);
        Task RemoveAsync(Guid accountId, string symbol);
    }
}
