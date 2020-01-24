using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface ICachedInstrumentService
    {
        public Task<Instrument> GetInstrumentAsync(Exchanges exchange, string symbol);
        public Task<IOrderBook> GetOrderBookAsync(Exchanges exchange, string symbol);
        public void UpdateInstrument(Exchanges exchange, string symbol, Instrument item);
    }
}
