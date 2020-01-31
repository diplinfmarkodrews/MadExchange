using MadXchange.Exchange.Dto.Http;
using MadXchange.Exchange.Interfaces;
using ServiceStack;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.RequestExecution
{
    public interface IRestRequestService
    {
        //private functions
        Task<WebResponseDto> SendGetAsync(Guid accountId, string url, string param, CancellationToken token);
        Task<WebResponseDto> SendPostAsync(Guid accountId, string url, string param, CancellationToken token);
        //public functions
        Task<WebResponseDto> SendGetAsync(string url);
    }
    public class RestRequestService : IRestRequestService
    {
        private readonly IRequestAccessService _requestAccessService;
        private readonly IRestRequestExecutionService _requestExecutionService;
        private readonly ISignRequests _signRequestService;

        public RestRequestService(IRestRequestExecutionService requestExecutionService, IRequestAccessService accessService, ISignRequests signRequestService) 
        {
            _requestAccessService = accessService;
            _requestExecutionService = requestExecutionService;
            _signRequestService = signRequestService;
        }
        private void CheckResponse(WebResponseDto response) 
        {
            if (response.result is null)
            {
                throw new ServiceResponseException(new ResponseStatus($"{response.ret_code}", response.ret_msg));
            }
        }
        /// <summary>
        /// private GetRequest, requests access before submitting a request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountId"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<WebResponseDto> SendGetAsync(Guid accountId, string url, string param, CancellationToken token = default)
        {            
            var permit = await _requestAccessService.RequestAccess(accountId, token);
            //if request was cancelled by CancellationToken, request gets aborted before execution
            if (!permit) return default;
            url = _signRequestService.SignRequest(accountId, url, param);
            var response = await _requestExecutionService.SendGetAsync(url).ConfigureAwait(false);
            _requestAccessService.UpdateAccountRequestCache(accountId, response);
            CheckResponse(response);
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="accountId"></param>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<WebResponseDto> SendPostAsync(Guid accountId, string url, string param, CancellationToken token = default)
        {
            var permit = await _requestAccessService.RequestAccess(accountId, token);
            //if request was cancelled by CancellationToken, request gets aborted before execution, no permission to send then, otherwise it will wait until access is granted
            if (!permit) return default;
            url = _signRequestService.SignRequest(accountId, url, param);
            var response = await _requestExecutionService.SendPostAsync(url).ConfigureAwait(false);
            _requestAccessService.UpdateAccountRequestCache(accountId, response);
            CheckResponse(response);
            return response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<WebResponseDto> SendGetAsync(string url)
        {
            var response = await _requestExecutionService.SendGetAsync(url).ConfigureAwait(false);
            return response;
        }
    }
}
