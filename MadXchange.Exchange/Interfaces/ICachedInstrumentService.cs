using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface ICachedInstrumentService
    {
        public Task<decimal> GetPriceAsync(Guid ExchnageId, string Symbol, OrderSide side);
        public Task<IOrderBook> GetOrderBookAsync(Guid ExchangeId, string Symbol);

    }
}
