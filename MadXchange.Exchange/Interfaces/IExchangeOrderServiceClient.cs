using MadXchange.Exchange.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IExchangeOrderServiceClient
    {

        public Task<IOrder> PlaceOrderAsync(CreateOrder order);
        public Task<IOrder> CancelOrderAsync(CancelOrder order);
        public Task<IOrder> UpdateOrderAsync(UpdateOrder order);
        public Task<IPosition> SetLeverage(SetLeverage leverage); 
    }
}