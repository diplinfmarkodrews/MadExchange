using System;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public interface IRequestAccessService
    {
        Task<string> SignRequest(Guid accountId, string url);
    }
    
}