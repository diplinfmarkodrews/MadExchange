using MadXchange.Exchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using System.Threading;
using MadXchange.Exchange.Dto.Http;

namespace MadXchange.Exchange.Services
{

    public class RestRequestService : IRestRequestService
    {
        private readonly IRequestAccessService _requestAccessService;
        private readonly IRestRequestExecutionService _requestExecutionService;
        public RestRequestService(IRestRequestExecutionService requestExecutionService, IRequestAccessService accessService) 
        {
            _requestAccessService = accessService;
            _requestExecutionService = requestExecutionService;
        }
        private void CheckResponse(WebResponseDto response) 
        {
            if (response.Result is null)
            {
                throw new ServiceResponseException(new ResponseStatus($"{response.Code}", response.Message));
            }
        }
        /// <summary>
        /// private GetRequest
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountId"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<T> SendGetAsync<T>(Guid accountId, string url, CancellationToken token = default)
        {
            
            var permit = await _requestAccessService.RequestAccess(accountId, token);
            if (!permit) throw new ServiceResponseException($"{accountId}: client did not acquire permission to send");
            var response = await _requestExecutionService.SendGetAsync(url).ConfigureAwait(false);
            _requestAccessService.UpdateAccountRequestCache(accountId, response);
            CheckResponse(response);
            return response.Result.ConvertTo<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountId"></param>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<T> SendPostAsync<T>(Guid accountId, string url, CancellationToken token = default)
        {
            await _requestAccessService.RequestAccess(accountId, token);            
            var response = await _requestExecutionService.SendPostAsync(url).ConfigureAwait(false);
            _requestAccessService.UpdateAccountRequestCache(accountId, response);
            return response.Result.ConvertTo<T>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> SendGetAsync<T>(string url)
        {
            var response = await _requestExecutionService.SendGetAsync(url).ConfigureAwait(false);
            return response.Result.ConvertTo<T>();

        }
    }
}
