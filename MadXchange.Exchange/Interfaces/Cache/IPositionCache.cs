using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces.Cache
{
    public interface IPositionCache
    {
        public void SetPosition(Guid accountId, string symbol, Position position);
        public Task<Position> GetPositionAsync(Guid accountId, string symbol);
    }
}
