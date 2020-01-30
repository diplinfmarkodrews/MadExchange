using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using System.Threading.Tasks;

namespace MadXchange.Connector.Interfaces
{
    public interface IExchangeRequestServiceClient
    {

        public Task<OrderDto> PlaceOrderAsync(CreateOrder order);
        public Task<OrderDto> CancelOrderAsync(CancelOrder order);
        public Task<OrderDto> UpdateOrderAsync(UpdateOrder order);
        public Task<LeverageDto> SetLeverage(SetLeverage leverage); 
    }
}