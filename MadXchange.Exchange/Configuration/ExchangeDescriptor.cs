using MadXchange.Common.Types;
using MadXchange.Exchange.Domain.Models;
using System;


namespace MadXchange.Exchange.Configuration
{
    public class ExchangeDescriptor : IIdentifiable
    {
        public Guid Id { get; }
        public bool IsEnabled { get; private set; }
        public string Name { get; private set; }
        public string WS { get; private set; }
        public string CS { get; private set; }
        public EndPoint<IMargin> RouteGetEquity { get; private set; }
        public EndPoint<IWalletHistory> RouteGetWalletHistory { get; private set; }
        public EndPoint<decimal> RouteGetLeverage { get; private set; }
        public EndPoint<IPosition> RouteGetPosition { get; private set; }
        public EndPoint<IOrder> RouteGetOrder { get; private set; }
        public EndPoint<IInstrument> RouteGetInstrument { get; private set; }
        public EndPoint<IOrderBook> RouteGetOrderBook { get; private set; }
        public EndPoint<IPosition> RoutePostLeverage { get; private set; }
        public EndPoint<IOrder> RoutePlaceOrder { get; private set; }
        public EndPoint<IOrder> RouteUpdateOrder { get; private set; }
        public EndPoint<IOrder> RouteDeleteOrder { get; private set; }
        

        public ExchangeDescriptor()
        {
            Id = Guid.NewGuid();
        }
        public ExchangeDescriptor(Guid id) 
        {
            Id = id;
        }
    }
}
