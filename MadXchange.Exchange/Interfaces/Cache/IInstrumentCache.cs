using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IInstrumentCache
    {
        public Task<Instrument> GetInstrumentAsync(Xchange exchange, string symbol);

        public Task<OrderBook> GetOrderBookAsync(Xchange exchange, string symbol);

        public long UpdateInstrument(Xchange exchange, string symbol, Instrument item);
    }
}