using MadXchange.Exchange.Contracts.Http;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Infrastructure.Cache;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Services.HttpRequests.RequestExecution
{
    public interface IRequestAccessService
    {
        Task<bool> RequestAccess(Guid accountId, CancellationToken token);
        void UpdateAccountRequestCache(Guid accountId, HttpResponseDto resDto);
    }

    public class RequestAccessService : IRequestAccessService
    {
        private readonly IAccountRequestCache _requestCache;
        private readonly ILogger _logger;
        private readonly long _minRequestTimeDiff = TimeSpan.FromMilliseconds(1000).Ticks;

        public RequestAccessService(IAccountRequestCache requestCache, ILogger<RequestAccessService> logger)
        {
            _requestCache = requestCache;
            _logger = logger;
        }

        public async Task<bool> RequestAccess(Guid accountId, CancellationToken token)
        {
            var resultLock = await _requestCache.LockAccount(accountId).ConfigureAwait(false);
            var acCacheObj = RequestAccountAccess(accountId);
            resultLock.Dispose();
            int rqQueue = acCacheObj.RequestQueue;
            long timeDiffInTicks = (acCacheObj.NextRequestTime - DateTime.UtcNow.Ticks);
            while (timeDiffInTicks > 0 && !token.IsCancellationRequested)
            {
                var delay = rqQueue * (int)(timeDiffInTicks * TimeSpan.TicksPerMillisecond);
                delay = delay > 0 ? delay : 0;
                await Task.Delay(delay, token).ConfigureAwait(false);
                acCacheObj = _requestCache.GetAccount(accountId);
                timeDiffInTicks = (int)(DateTime.UtcNow.Ticks - acCacheObj?.NextRequestTime);
            }
            return !token.IsCancellationRequested;
        }

        /// <summary>
        /// clients register their request first to gain access
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private AccountRequestCacheObject RequestAccountAccess(Guid accountId)
        {
            //var lockObj = _requestCache.LockAccount(accountId);
            var accountCacheObject = _requestCache.GetAccount(accountId);
            accountCacheObject.RequestQueue++;           
            accountCacheObject.Timestamp = DateTime.UtcNow.Ticks;           
            _requestCache.SetAccount(accountCacheObject);
            return accountCacheObject;
        }

        public void UpdateAccountRequestCache(Guid accountId, HttpResponseDto resDto)
        {
            var acCacheObj = _requestCache.GetAccount(accountId);            
            acCacheObj.LastRateLimit = resDto.RateLimit;
            acCacheObj.RateLimitStatus = resDto.RateLimitStatus;
            acCacheObj.LastRequestTime = resDto.Timestamp;            
            acCacheObj.NextRequestTime = acCacheObj.LastRequestTime + _minRequestTimeDiff;
            acCacheObj.RequestQueue --;
            if(acCacheObj.RequestQueue < 0)
                acCacheObj.RequestQueue = 0;
            acCacheObj.Timestamp = DateTime.UtcNow.Ticks;
            _requestCache.SetAccount(acCacheObj);
        }
    }
}