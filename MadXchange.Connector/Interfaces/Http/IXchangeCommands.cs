using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using System.Threading.Tasks;

namespace MadXchange.Connector.Interfaces
{
    /// <summary>
    /// Executes orders on command input.
    /// can implement price adaption, but how to implement cancellation?
    /// </summary>
    public interface IXchangeCommands
    {
        public Task<Order> PlaceOrderAsync(CreateOrder order);

        public Task<Order> CancelOrderAsync(CancelOrder order);

        public Task<Order> UpdateOrderAsync(UpdateOrder order);

        public Task<LeverageDto> SetLeverage(SetLeverage leverage);
    }
}