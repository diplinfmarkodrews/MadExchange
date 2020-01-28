using MadXchange.Exchange.Dto.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public interface IRequestAccessService
    {        
        Task<bool> RequestAccess(Guid accountId, CancellationToken token);
        void UpdateAccountRequestCache(Guid accountId, WebResponseDto resDto);
    }
    
}