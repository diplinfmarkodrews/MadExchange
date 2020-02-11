using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;

namespace MadXchange.Exchange.Contracts
{
    public class OrderPutRequestDto : IOrderPutRequest
    {
        public Guid AccountId { get; private set; }
        public Xchange Exchange { get; set; }
        public string Symbol { get; set; }
        public string OrderId { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
    }
}