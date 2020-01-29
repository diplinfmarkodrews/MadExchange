using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IRestRequestService
    {
        //private functions
        Task<T> SendGetAsync<T>(Guid accountId, string url, CancellationToken token);        
        Task<T> SendPostAsync<T>(Guid accountId, string url, CancellationToken token);
        //public functions
        Task<T> SendGetAsync<T>(string url);
    }
}
