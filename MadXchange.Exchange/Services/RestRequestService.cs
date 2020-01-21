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
    public class RestRequestService : IRestRequestService
    {
        private IRequestAccessService _exchangeRequestAccessService;
        //private IServiceClientAsync _serviceClient;
        private ILogger _logger;
        public RestRequestService(//IServiceClientAsync client,
                                  IRequestAccessService requestAccessService,
                                  ILogger<RestRequestService> logger) 
        {
          //  _serviceClient = client;    
            
            _exchangeRequestAccessService = requestAccessService;
            _logger = logger;
        }
        public async Task<string> SendGetAsync(Exchanges exchange, string url)
        {
            try
            {

                var resp = await url.GetJsonFromUrlAsync().ConfigureAwait(false);
                
                var resDto = resp.ConvertTo<WebResponseDto>();
                if(resDto.Code == 0)
                {

                    return resDto.Result;

                }
                return resDto.Message;
            } 
            catch (Exception err) 
            {
                return err.Message;
            }
            
        }
        public async Task<string> SendPrivateGetAsync(Exchanges exchange, Guid accountId, string url) 
        {
            _exchangeRequestAccessService.Get
            throw new NotImplementedException();
        }

        
    }
}
