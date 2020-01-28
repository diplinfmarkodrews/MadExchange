using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces.Cache
{
    public interface IOrderCache
    {
        public bool AddOrder(Guid accountId, string symbol, string orderId, Order order);
        public bool RemoveOrder(Guid accountId, string symbol, string orderId);
        public bool UpdateOrder(Guid accountId, string symbol, string orderId, Order order);
        public Task<Order> GetOrderAsync(Guid accountId, string symbol, string orderId);
    }
}
