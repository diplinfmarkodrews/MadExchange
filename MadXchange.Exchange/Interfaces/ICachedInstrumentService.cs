using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface ICachedPriceService
    {
        public Task<IInstrument> GetPriceAsync(int ExchnageId, string Symbol, OrderSide side);
        public Task<IOrderBook> GetOrderBookAsync(int ExchangeId, string Symbol);

    }
}
