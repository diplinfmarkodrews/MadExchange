using MadXchange.Connector.Domain.Models;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Contracts.XchangeData;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using ServiceStack.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Handler
{

    public sealed class CacheDataHandler : ConnectionDataHandler
    {

        private readonly IInstrumentCache _instrumentCache;
        private readonly IOrderBookCache _orderBookCache;
        private readonly IPositionCache _positionCache;
        private readonly IOrderCache _orderCache;
        private readonly ILogger _logger;
        
        public CacheDataHandler(IInstrumentCache instrumentCache,
                                 IOrderBookCache orderBookCache,
                                  IPositionCache positionCache,
                                     IOrderCache orderCache,
                       ILogger<CacheDataHandler> logger)
        
            
        {

            _instrumentCache = instrumentCache;
            _orderBookCache = orderBookCache;
            _positionCache = positionCache;
            _orderCache = orderCache;
            _logger = logger;
        
        }

        [MethodImpl(methodImplOptions: MethodImplOptions.AggressiveInlining)]
        public override Task HandleDataAsync(IDataPack data)
            => data is SocketUpdatePack ? HandleDataAsync(data as SocketUpdatePack) : HandleDataAsync(data as SocketMsgPack);

        /// <summary>
        /// implementation of a updating a single data msg pack
        /// </summary>
        /// <param name="socketMsgPack"></param>
        /// <returns></returns>
        private Task HandleDataAsync(SocketMsgPack socketMsgPack)
        {
            _logger.LogDebug($"socket message received:\n{socketMsgPack.Dump()}");
            var dataType = socketMsgPack.Data.GetType();
            if(dataType == typeof(InstrumentDto))
            {
                var instrument = Instrument.FromModel((InstrumentDto)socketMsgPack.Data);
                _instrumentCache.Update(id: socketMsgPack.Id,
                                  exchange: socketMsgPack.Exchange,
                                    symbol: instrument.Symbol,
                                 timeStamp: socketMsgPack.Timestamp,
                                      item: instrument);
                
                return Task.CompletedTask;                                           
            }

            if(dataType == typeof(OrderBookDto[]))
            {
                var orderBook = OrderBook.FromModel((OrderBookDto[])socketMsgPack.Data);
                _orderBookCache.InsertOrderBook(id: socketMsgPack.Id,
                                          exchange: socketMsgPack.Exchange,
                                            symbol: orderBook[0].Symbol,
                                         timeStamp: socketMsgPack.Timestamp,
                                            insert: orderBook);
                
                return Task.CompletedTask;
            }

            if (dataType == typeof(OrderDto))
            {
                _orderCache.InsertOrder(id: socketMsgPack.Id,
                                 timestamp: socketMsgPack.Timestamp,
                                    insert: Order.FromModel((OrderDto)socketMsgPack.Data));

                return Task.CompletedTask;
            }

            if(dataType == typeof(PositionDto))
            {
                _positionCache.Insert(id: socketMsgPack.Id,
                               timestamp: socketMsgPack.Timestamp,
                                position: Position.FromModel((PositionDto)socketMsgPack.Data));
               
                return Task.CompletedTask;
            }

            if (dataType == typeof(MarginDto))
            {
                _positionCache.Insert(id: socketMsgPack.Id,
                               timestamp: socketMsgPack.Timestamp,
                                position: Position.FromModel((MarginDto)socketMsgPack.Data));

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketMsgPack"></param>
        /// <returns></returns>
        private Task HandleDataAsync(SocketUpdatePack socketMsgPack)
        {
            _logger.LogDebug($"socket update message received:\n{socketMsgPack.Dump()}");
            var dataType = socketMsgPack.Update.GetType();
            if (dataType == typeof(InstrumentDto[]))
            {
                var instrument = Instrument.FromModel((InstrumentDto[])socketMsgPack.Update);
                _instrumentCache.Update(id: socketMsgPack.Id,
                                  exchange: socketMsgPack.Exchange,
                                    symbol: instrument[0]?.Symbol,
                                 timeStamp: socketMsgPack.Timestamp,
                                      item: instrument[0]);
                
                return Task.CompletedTask;

            }

            if (dataType == typeof(OrderBookDto[]))
            {
                var orderBookUpdates = OrderBook.FromModel((OrderBookDto[])socketMsgPack.Update);
                var orderBookInserts = OrderBook.FromModel((OrderBookDto[])socketMsgPack.Insert);
                var orderBookDeletes = OrderBook.FromModel((OrderBookDto[])socketMsgPack.Delete);
                _orderBookCache.Update(id: socketMsgPack.Id,
                                 exchange: socketMsgPack.Exchange,
                                   symbol: orderBookUpdates.Length > 0 ? orderBookUpdates[0].Symbol : orderBookInserts.Length > 0 ? orderBookInserts[0].Symbol : orderBookDeletes[0].Symbol,
                                timeStamp: socketMsgPack.Timestamp,
                                   insert: orderBookInserts,
                                   update: orderBookUpdates,
                                   delete: orderBookDeletes);
                
                return Task.CompletedTask;

            }

            if (dataType == typeof(OrderDto[])) 
            {                
                _orderCache.Update(id: socketMsgPack.Id,
                            timeStamp: socketMsgPack.Timestamp,
                               insert: Order.FromModel((OrderDto[])socketMsgPack.Insert),
                               update: Order.FromModel((OrderDto[])socketMsgPack.Update),
                               delete: Order.FromModel((OrderDto[])socketMsgPack.Delete));
                
                return Task.CompletedTask;
            }

            if (dataType == typeof(PositionDto[]))
            {
                _positionCache.Update(id: socketMsgPack.Id,    
                               timeStamp: socketMsgPack.Timestamp,
                                  insert: Position.FromModel((PositionDto[])socketMsgPack.Insert),
                                  update: Position.FromModel((PositionDto[])socketMsgPack.Update),
                                  delete: Position.FromModel((PositionDto[])socketMsgPack.Delete));

                return Task.CompletedTask;
            }

            if (dataType == typeof(MarginDto))
            {
                _positionCache.Update(id: socketMsgPack.Id,
                               timeStamp: socketMsgPack.Timestamp,
                                  insert: Position.FromModel((MarginDto[])socketMsgPack.Insert),
                                  update: Position.FromModel((MarginDto[])socketMsgPack.Update),
                                  delete: Position.FromModel((MarginDto[])socketMsgPack.Delete));

                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
