using MadXchange.Exchange.Dto.Http;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Interfaces
{
    public interface IRestRequestExecutionService
    {
        Task<WebResponseDto> SendGetAsync(string url);
        Task<WebResponseDto> SendPostAsync(string url);
    }
}