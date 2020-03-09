using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;

namespace MadXchange.Exchange.Contracts
{
    public class OrderPostRequestDto : IOrderPostRequest
    {
        public Guid AccountId { get; private set; }
        public Xchange Exchange { get; set; }
        public string Symbol { get; set; }
        public decimal? Quantity { get; set; }
        public OrderSide? Side { get; set; }
        public decimal? Price { get; set; }
        public OrderType? OrdType { get; set; }
        public TimeInForce? TimeInForce { get; set; }
        public IEnumerable<ExecInst> Execs { get; set; }

        public OrderPostRequestDto(Guid accountId, Xchange exchange, string symbol, decimal? qty, decimal? price, OrderSide? side, OrderType ordType, TimeInForce tif, IEnumerable<ExecInst> execs)
        {
            AccountId = accountId;
            Exchange = exchange;
            Symbol = symbol;
            Quantity = qty;
            Price = price;
            Side = side;
            OrdType = ordType;
            TimeInForce = tif;
            Execs = execs;
        }
    }
}