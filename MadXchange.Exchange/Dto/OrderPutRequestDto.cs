using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using System;

namespace MadXchange.Exchange.Dto
{
    public class OrderPutRequestDto : IOrderPutRequest
    {
        public Guid AccountId { get; private set; }
        public Exchanges Exchange { get; set; }
        public string Symbol { get; set; }
        public string OrderId { get; set; }
        public decimal? Qty { get; set; }
        public decimal? Price { get; set; }
    }
}
