using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IInstrumentCache
    {
        public Task<Instrument> GetInstrumentAsync(Exchanges exchange, string symbol);
        public Task<OrderBook> GetOrderBookAsync(Exchanges exchange, string symbol);
        public long UpdateInstrument(Exchanges exchange, string symbol, Instrument item);
    }
}
