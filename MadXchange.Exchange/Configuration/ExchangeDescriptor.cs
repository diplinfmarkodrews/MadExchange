using MadXchange.Common.Types;
using Convey.Types;
using MadXchange.Exchange.Domain.Models;
using ServiceStack;
using ServiceStack.IO;
using System;
using System.Collections.Generic;
using MadXchange.Exchange.Types;
using MadXchange.Exchange.Dto;

namespace MadXchange.Exchange.Configuration
{
   

    public class ExchangeDescriptor : IIdentifiable<int>
    {
        public int Id { get; set; }       
        public string Name { get; set; }
        public string SocketUrl { get; set; }
        public string BaseUrl { get; set; }
        public IDictionary<string, IEndpoint> EndPoints { get; set; }
        
        public EndPoint<IMargin> RouteGetEquity { get; set; }
        public EndPoint<IWalletHistory> RouteGetWalletHistory { get; set; }
        public EndPoint<LeverageDto> RouteGetLeverage { get; set; }
        public EndPoint<IPosition> RouteGetPosition { get; set; }
        public EndPoint<IOrder> RouteGetOrder { get; set; }
        public EndPoint<Instrument> RouteGetInstrument { get; set; }
        

        //public EndPoint<IOrderBook> RouteGetOrderBook { get; set; }
        public EndPoint<LeverageDto> RoutePostLeverage { get; set; }
        public EndPoint<IOrder> RoutePlaceOrder { get; set; }
        public EndPoint<IOrder> RouteUpdateOrder { get; set; }
        public EndPoint<IOrder> RouteDeleteOrder { get; set; }   
        public EndPoint<IOrder> RouteDeleteAllOrders { get; set; }


        public ExchangeDescriptor(int id) 
        {
            Id = id;
        }

        public ExchangeDescriptor()
        {
        }

   
    }
}
