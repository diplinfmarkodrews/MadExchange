using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IRestRequestService
    {
        Task<T> SendGetAsync<T>(Guid accountId, string url, string parameter);
        Task<T> SendGetAsync<T>(string url, string parameter);
        Task<T> SendPostAsync<T>(Guid accountId, string url, string parameter);
    }
}
