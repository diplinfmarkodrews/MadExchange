using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MadXchange.Exchange.Dto
{
    public class OrderDto 
    {
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public string OrderId { get; set; }
        public OrderStatus OrdStatus { get; set; }
        public OrderType OrdType { get; set; }
        public decimal Price { get; set; }
    }
}
