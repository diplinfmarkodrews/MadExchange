using MadXchange.Exchange.Contracts.Http;
using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Types;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.HttpRequests.RequestExecution
{
    public interface IRequestExecutionService
    {
        Task<HttpResponseDto> SendRequestObjectAsync(XchangeRequestObject routeObject);

        Task<HttpResponseDto> SendGetAsync(string url);
    }

    public class RequestExecutionService : IRequestExecutionService
    {
        private ILogger _logger;

        public RequestExecutionService(ILogger<RequestExecutionService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// send public GET Request to given url and convert to base response type
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<HttpResponseDto> SendGetAsync(string url)
        {
            try
            {
                var resp = await url.GetJsonFromUrlAsync().ConfigureAwait(false);
                return resp.FromJson<HttpResponseDto>();
            }
            catch (Exception err)
            {
                _logger.LogError($"Error sending get request to url: {url}\n{err.Message}", err);
            }
            return default;
        }

        public async Task<HttpResponseDto> SendRequestObjectAsync(XchangeRequestObject routeObject)
        {
            try
            {
                string response = string.Empty;
                if(routeObject.Method == "Get")                
                    response = await routeObject.GetSignedUrl().GetJsonFromUrlAsync().ConfigureAwait(false);
                else
                    response = await routeObject.Url.SendStringToUrlAsync(routeObject.Method, routeObject.ToOrderedJson(), contentType: "application/json").ConfigureAwait(false);
                return response.FromJson<HttpResponseDto>();
            }
            catch (Exception ex)
            {               
                _logger.LogError($"Error sending {routeObject} request: {ex.Message}", ex, routeObject);
                throw;
            }
           
        }
    }
}