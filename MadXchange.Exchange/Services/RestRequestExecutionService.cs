using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Interfaces;
using MadXchange.Exchange.Services;
using ServiceStack;
using System;
using System.Net;
using Convey.HTTP;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MadXchange.Exchange.Dto.Http;

namespace MadXchange.Exchange.Domain.Cache
{
    public class RestRequestExecutionService : IRestRequestExecutionService
    {
        private IRequestAccessService _exchangeRequestAccessService;
        
        private ILogger _logger;
        public RestRequestExecutionService(
                IRequestAccessService requestAccessService,
                ILogger<RestRequestExecutionService> logger
            ) 
        {            
            _exchangeRequestAccessService = requestAccessService;
            _logger = logger;
        }
        
        /// <summary>
        /// send public GET Request to given url and convert to base requestresponsetype
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<WebResponseDto> SendGetAsync(string url)
        {
            try
            {                
                var resp = await url.GetJsonFromUrlAsync().ConfigureAwait(false);                
                var resDto = resp.ConvertTo<WebResponseDto>();
                return resDto;
            } 
            catch (Exception err) 
            {
                _logger.LogError($"Error sending get request: {err.Message}", err);
            }
            return null;
        }
        /// <summary>
        /// Sends private, ratelimited requests
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<WebResponseDto> SendGetAsync(Guid accountId, string url) 
        {
            try
            {                
                await _exchangeRequestAccessService.RequestAccess(accountId);               
                var resp = await url.GetJsonFromUrlAsync().ConfigureAwait(false);
                var resDto = resp.ConvertTo<WebResponseDto>();
                return resDto;
            }
            catch (Exception err)
            {
                _logger.LogError($"Error sending get request: {err.Message}", err);
            }
            return null;
        }

        

        public async Task<WebResponseDto> SendPostAsync(Guid accountId, string url)
        {
            try
            {
                await _exchangeRequestAccessService.RequestAccess(accountId);
                var resp = await url.GetJsonFromUrlAsync().ConfigureAwait(false);
                var resDto = resp.ConvertTo<WebResponseDto>();
                _exchangeRequestAccessService.UpdateAccountRequestCache(accountId, resDto);
                return resDto;
            }
            catch (Exception err)
            {
                _logger.LogError($"Error sending get request: {err.Message}", err);
            }
            return null;
        }
    }
}
