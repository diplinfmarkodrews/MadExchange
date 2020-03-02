using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Contracts;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Services.HttpRequests;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{
    /// <summary>
    /// Manages POST Request Commands to the exchange
    /// </summary>
    public class CommandExecutionService : IXchangeCommands
    {
        private readonly IPositionRequestService _positionRequestService;
        private readonly IOrderRequestService _orderRequestService;

        /// <summary>
        /// since only post requests are cancelable here is the right place to generate the CancellationTokens
        /// </summary>
        private static readonly Dictionary<Guid, CancellationToken> _requestCancelTokenDic = new Dictionary<Guid, CancellationToken>();
        private readonly ILogger _logger;

        public CommandExecutionService(IPositionRequestService positionRequestService, 
                                          IOrderRequestService orderRequestService, 
                              ILogger<CommandExecutionService> logger)
        {
            _positionRequestService = positionRequestService;
            _orderRequestService = orderRequestService;
            _logger = logger;
        }

        public async Task<Order> CancelOrderAsync(CancelOrder order)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> PlaceOrderAsync(CreateOrder order)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> UpdateOrderAsync(UpdateOrder order)
        {
            throw new NotImplementedException();
        }

        public async Task<LeverageDto> SetLeverage(SetLeverage leverage)
        {
            throw new NotImplementedException();
        }
    }
}