using MadXchange.Exchange.Domain.Models;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces.Cache
{
    public interface IOrderCache
    {
        public void AddOrder(Guid accountId, string symbol, string orderId, Order order);

        public bool UpdateOrder(Guid accountId, string symbol, string orderId, Order order);

        public void RemoveOrder(Guid accountId, string symbol, string orderId);

        public Task<Order> GetOrderAsync(Guid accountId, string symbol, string orderId);
    }
}