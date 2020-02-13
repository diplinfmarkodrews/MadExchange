using MadXchange.Exchange.Contracts.Http;
using MadXchange.Exchange.Domain.Models;
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
                string resp = await routeObject.Url.SendStringToUrlAsync(routeObject.Method, routeObject.ToOrderedJson()).ConfigureAwait(false);
                return resp.FromJson<HttpResponseDto>();
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
                _logger.LogError($"Error sending {routeObject} request: {ex.Message}", ex, routeObject);
            }
            return default;
        }
    }
}