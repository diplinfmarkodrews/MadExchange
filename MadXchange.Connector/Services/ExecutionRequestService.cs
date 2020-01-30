using MadXchange.Connector.Interfaces;
using MadXchange.Connector.Messages.Commands;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Dto;
using MadXchange.Exchange.Services.HttpRequests;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MadXchange.Connector.Services
{

    /// <summary>
    /// Manages Request Commands
    /// </summary>
    public class ExecutionRequestService : IExchangeRequestServiceClient
    {

        private readonly IPositionRequestService _positionRequestService;
        private readonly IOrderRequestService _orderRequestService;
        
        
        private readonly ILogger _logger;

        public ExecutionRequestService(IPositionRequestService positionRequestService, IOrderRequestService orderRequestService, ILogger<ExecutionRequestService> logger) 
        {
            _positionRequestService = positionRequestService;
            _orderRequestService = orderRequestService;
            _logger = logger;
        }

        public async Task<OrderDto> CancelOrderAsync(CancelOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> PlaceOrderAsync(CreateOrder order)
        {
            throw new NotImplementedException();
        }

        public Task<LeverageDto> SetLeverage(SetLeverage leverage)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> UpdateOrderAsync(UpdateOrder order)
        {
            throw new NotImplementedException();
        }
    }
}
