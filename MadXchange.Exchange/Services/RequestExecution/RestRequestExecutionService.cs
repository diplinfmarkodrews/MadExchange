using MadXchange.Exchange.Dto.Http;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.RequestExecution
{
    public interface IRestRequestExecutionService
    {
        Task<WebResponseDto> SendGetAsync(string url);
        Task<WebResponseDto> SendPostAsync(string url);
    }
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
                var resp = await url.GetJsonFromUrlAsync().ContinueWith(f=> f.Result.ConvertTo<WebResponseDto>()).ConfigureAwait(false);                
                
                return resp;
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
