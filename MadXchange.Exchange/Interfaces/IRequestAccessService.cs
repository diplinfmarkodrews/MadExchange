using MadXchange.Exchange.Dto.Http;
using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public interface IRequestAccessService
    {
        Task<string> SignRequest(Guid accountId, string url);
        Task RequestAccess(Guid accountId);
        void UpdateAccountRequestCache(Guid accountId, WebResponseDto resDto);
    }
    
}