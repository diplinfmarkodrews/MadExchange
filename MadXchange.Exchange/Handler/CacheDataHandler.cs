using MadXchange.Connector.Domain.Models;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Handler
{

    public class CacheDataHandler : ConnectionDataHandler
    {

        private readonly IInstrumentCache _instrumentCache;
        //private readonly IPositionCache _positionCache;
        //private readonly IOrderCache _orderCache;
        //private readonly IMarginCache _marginCache;
        private readonly ILogger _logger;
        
        public CacheDataHandler(IInstrumentCache instrumentCache,
                                    //IPositionCache positionCache,
                                    //   IOrderCache orderCache,
                                    //  IMarginCache marginCache,
                                        ILogger<CacheDataHandler> logger)
        
            
        {

            _instrumentCache = instrumentCache;
            //_positionCache = positionCache;
            //_marginCache = marginCache;
            //_orderCache = orderCache;
            _logger = logger;
        
        }
        public override Task HandleDataAsync(SocketMsgPack socketMsgPack)
        {
            _logger.LogDebug($"socket message received:\n{socketMsgPack}");

            if(socketMsgPack.Data.GetType() == typeof(InstrumentDto))
            {

                var instrument = Instrument.FromModel((InstrumentDto)socketMsgPack.Data);
                return Task.FromResult(_instrumentCache.UpdateInstrument(id: socketMsgPack.Id,
                                                                   exchange: socketMsgPack.Exchange,
                                                                     symbol: instrument.Symbol,
                                                                  timeStamp: socketMsgPack.Timestamp,
                                                                       item: instrument));
                                            
            }

            return Task.CompletedTask;
        }
    }
}
