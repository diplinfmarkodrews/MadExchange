using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Infrastructure.Cache;
using MadXchange.Exchange.Infrastructure.Stores;
using MadXchange.Exchange.Messages.Commands.OrderService;
using MadXchange.Exchange.Services.HttpRequests;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    /// <summary>
    /// Executes orders on command input.
    /// can implement price adaption, but how to implement cancellation? ==>box command with cancellation token
    /// </summary>
    public interface IXchangeCommands
    {
        Task<Order> PlaceOrderAsync(CreateOrder order);
        Task<Order> CancelOrderAsync(CancelOrder order);
        Task<Order> UpdateOrderAsync(UpdateOrder updateOrder);
        Task<Position> SetLeverage(SetLeverage postLeverage);
        bool CancelCommand(Guid commandId);
    }
    /// <summary>
    /// Manages POST Request Commands to the exchange
    /// </summary>
    public sealed class CommandExecutionService : IXchangeCommands, IService
    {
        private readonly IPositionRequestService _positionRequestService;
        private readonly IOrderRequestService _orderRequestService;
        private readonly IPositionCache _positionCache;
        private readonly IOrderCache _orderCache;
        private readonly ICommandStore _commandStore;
        /// <summary>
        /// since only post requests are cancelable here is the right place to generate the CancellationTokens
        /// we generate tokens for each command, reference them by their ids and store them in the commandstore.
        /// on cancel we retrieve the token from the command store and cancel it.
        /// </summary>
        
        private readonly ILogger _logger;

        public CommandExecutionService(IPositionRequestService positionRequestService, 
                                          IOrderRequestService orderRequestService, 
                                                IPositionCache positionCache,
                                                   IOrderCache orderCache,
                                                 ICommandStore commandStore,
                              ILogger<CommandExecutionService> logger) 
        {

            _positionRequestService = positionRequestService;
            _orderRequestService = orderRequestService;
            _positionCache = positionCache;
            _orderCache = orderCache;
            _commandStore = commandStore;
            _logger = logger;
        }

        public async Task<Order> CancelOrderAsync(CancelOrder cancelOrder)
        {
            var token = _commandStore.RegisterCommand(cancelOrder.Id, cancelOrder);
            var response = await _orderRequestService.DeleteOrderAsync(exchange: cancelOrder.Exchange,
                                                                      accountId: cancelOrder.AccountId,
                                                                         symbol: cancelOrder.Symbol,
                                                                        orderId: cancelOrder.OrderId,
                                                                          token: token).ConfigureAwait(false);

            _commandStore.ClearCommand(cancelOrder.Id);
            var orderResult = Order.FromModel(response);
            await _orderCache.UpdateAsync(cancelOrder.AccountId, orderResult.Timestamp, orderResult);
            return orderResult;

        }

        public async Task<Order> PlaceOrderAsync(CreateOrder createOrder)
        {
            var token = _commandStore.RegisterCommand(createOrder.Id, createOrder);
            var response = await _orderRequestService.PostNewOrderAsync(new OrderPostRequestDto(accountId: createOrder.AccountId,
                                                                                                 exchange: createOrder.Exchange,
                                                                                                   symbol: createOrder.Symbol,                                                                                                    
                                                                                                      qty: createOrder.Amount,
                                                                                                    price: createOrder.Price,
                                                                                                     side: createOrder.Side,
                                                                                                  ordType: createOrder.OrderType.GetValueOrDefault(OrderType.Market),
                                                                                                      tif: createOrder.TimeInForce.GetValueOrDefault(TimeInForce.ImmediateOrCancel),
                                                                                                    execs: createOrder.Execs),
                                                                                                    token: token).ConfigureAwait(false);

            _commandStore.ClearCommand(createOrder.Id);
            var order = Order.FromModel(response);
            // todo error handling
            //on success the order is inserted into our order cache
            await _orderCache.InsertAsync(createOrder.AccountId, response.Timestamp, order);
            
            return order;

        }

        public async Task<Order> UpdateOrderAsync(UpdateOrder updateOrder)
        {
            var token = _commandStore.RegisterCommand(updateOrder.Id, updateOrder);
            var response = await _orderRequestService.PostUpdateOrderAsync(new OrderPutRequestDto(accountId: updateOrder.AccountId,
                                                                                                   exchange: updateOrder.Exchange,
                                                                                                     symbol: updateOrder.Symbol,
                                                                                                    orderId: updateOrder.OrderId,
                                                                                                        qty: updateOrder.Amount,
                                                                                                      price: updateOrder.Price), 
                                                                                                      token: token).ConfigureAwait(false);

            _commandStore.ClearCommand(updateOrder.Id);
            var order = Order.FromModel(response);
            if (string.IsNullOrEmpty(response.OrdRejReason))
            {
                
                //todo error handling, update cache..
                //

                await _orderCache.UpdateAsync(updateOrder.AccountId, response.Timestamp, order).ConfigureAwait(false);
                
            }
            return order;
        }

        public async Task<Position> SetLeverage(SetLeverage setLeverage)
        {
            var token = _commandStore.RegisterCommand(setLeverage.Id, setLeverage);
            var response = await _positionRequestService.PostLeverageAsync(accountId: setLeverage.AccountId, 
                                                                            exchange: setLeverage.Exchange, 
                                                                              symbol: setLeverage.Symbol, 
                                                                            leverage: setLeverage.Leverage, 
                                                                               token: token).ConfigureAwait(false);

            _commandStore.ClearCommand(setLeverage.Id);                        
            var pos  = Position.FromModel(response);
            await _positionCache.UpdateLeverageAsync(setLeverage.AccountId, response.Timestamp, pos).ConfigureAwait(false);
            return pos;
        }

       

        public bool CancelCommand(Guid commandId)
        {
            var command = _commandStore.GetCommand(commandId);
            if (command is null)
                return false;
            command.CancellationSource.Cancel();
            //command.Dispose();
            return true;
        }
    }
}