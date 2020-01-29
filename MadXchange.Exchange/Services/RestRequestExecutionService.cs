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
using System.Threading;

namespace MadXchange.Exchange.Domain.Cache
{
    public class RestRequestExecutionService : IRestRequestExecutionService
    {

        private ILogger _logger;
        public RestRequestExecutionService(ILogger<RestRequestExecutionService> logger) 
        {                        
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
                

        public async Task<WebResponseDto> SendPostAsync(string url)
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
    }
}
