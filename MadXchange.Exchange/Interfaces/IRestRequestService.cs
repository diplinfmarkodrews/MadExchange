using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IRestRequestService
    {
        Task<string> SendGetAsync(Domain.Models.Exchanges exchange, string url);  
    }
}