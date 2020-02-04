using MadXchange.Exchange.Contracts.Http;
using Microsoft.Extensions.Logging;
using ServiceStack;
using ServiceStack.Text;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.RequestExecution
{
    public interface IRestRequestExecutionService
    {
        Task<WebResponseDto> SendGetAsync(string url);
        Task<WebResponseDto> SendPostAsync(string url, string param);
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
                var resp = await url.GetJsonFromUrlAsync().ConfigureAwait(false);                               
                return TypeSerializer.DeserializeFromString<WebResponseDto>(resp);
            } 
            catch (Exception err) 
            {
                _logger.LogError($"Error sending get request: {err.Message}", err);
            }
            return default;
        }
                

        public async Task<WebResponseDto> SendPostAsync(string url ,string param)
        {
            try
            {
                var resp = await url.PostJsonToUrlAsync(param).ConfigureAwait(false);
                return TypeSerializer.DeserializeFromString<WebResponseDto>(resp);
            }
            catch (Exception ex)
            {
                //var knownError = ex.IsBadRequest()
                //                    || ex.IsNotFound()
                //                    || ex.IsUnauthorized()
                //                    || ex.IsForbidden()
                //                    || ex.IsInternalServerError();

                //var isAnyClientError = ex.IsAny400();
                //var isAnyServerError = ex.IsAny500();

                //HttpStatusCode? errorStatus = ex.GetStatus();
                //string errorBody = ex.GetResponseBody();
                _logger.LogError($"Error sending get request: {ex.Message}", ex);
            }
            return default;
        }
    }
}
