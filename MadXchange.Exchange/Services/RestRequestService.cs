using MadXchange.Exchange.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public class RestRequestService : IRestRequestService
    {

        public RestRequestService() 
        {
        
        }
        public Task<T> SendGetAsync<T>(Guid accountId, string url, string parameter)
        {
            throw new NotImplementedException();
        }
        public Task<T> SendPostAsync<T>(Guid accountId, string url, string parameter)
        {
            throw new NotImplementedException();
        }
        public Task<T> SendGetAsync<T>(string url, string parameter)
        {
            throw new NotImplementedException();
        }
    }
}
