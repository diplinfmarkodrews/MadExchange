using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Dto.Http;
using MadXchange.Exchange.Interfaces.Cache;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services
{
    public class RequestAccessService : IRequestAccessService
    {
        
        private readonly IAccountRequestCache _requestCache;
        private readonly long _minRequestTimeDiff = TimeSpan.FromMilliseconds(1000).Ticks;
        public RequestAccessService(IAccountRequestCache requestCache) 
        {
            _requestCache = requestCache;
        }
        public async Task<bool> RequestAccess(Guid accountId, CancellationToken token)
        {
            var acCacheObj = RequestAccountAccess(accountId);
            int rqQueue = acCacheObj.RequestQueue;
            long timeDiffInTicks = (DateTime.UtcNow.Ticks - acCacheObj.NextRequestTime);
            while (timeDiffInTicks < 0) 
            {
                await Task.Delay(rqQueue * (int)(Math.Abs(timeDiffInTicks) * TimeSpan.TicksPerMillisecond), token);
                acCacheObj = _requestCache.GetAccount(accountId);                
                timeDiffInTicks = (int)(DateTime.UtcNow.Ticks - acCacheObj?.NextRequestTime);
            }
            return !token.IsCancellationRequested;
        }

        private AccountCacheObject RequestAccountAccess(Guid accountId)
        {
            var accountCacheObject = _requestCache.GetAccount(accountId);
            if (accountCacheObject is null) accountCacheObject = new AccountCacheObject(accountId);
            accountCacheObject.RequestQueue++;
            accountCacheObject.Timestamp = DateTime.UtcNow.Ticks;
            _requestCache.SetAccount(accountCacheObject);
            return accountCacheObject;
        }


        public void UpdateAccountRequestCache(Guid accountId, WebResponseDto resDto)
        {
            var acCacheObj = _requestCache.GetAccount(accountId);
            if (acCacheObj is null)
            {
                acCacheObj = new AccountCacheObject(accountId);
                acCacheObj.RequestQueue++;
            }
            acCacheObj.LastRateLimit = resDto.RateLimit;
            acCacheObj.RateLimitStatus = resDto.RateLimitStatus;            
            var dtDto = DateTime.Parse(resDto.TimeNow);
            acCacheObj.LastRequestTime = dtDto == default ? resDto.Timestamp : dtDto.Ticks;
            acCacheObj.NextRequestTime = acCacheObj.LastRequestTime + _minRequestTimeDiff;
            acCacheObj.RequestQueue--;
            acCacheObj.Timestamp = DateTime.UtcNow.Ticks;
            _requestCache.SetAccount(acCacheObj);
        }
    }
}
