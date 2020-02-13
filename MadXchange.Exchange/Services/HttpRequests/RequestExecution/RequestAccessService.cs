using MadXchange.Exchange.Contracts.Http;
using MadXchange.Exchange.Domain.Cache;
using MadXchange.Exchange.Interfaces.Cache;
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
            var acCacheObj = RequestAccountAccess(accountId);
            int rqQueue = acCacheObj.RequestQueue;
            long timeDiffInTicks = (DateTime.UtcNow.Ticks - acCacheObj.NextRequestTime);
            while (timeDiffInTicks < 0 || !token.IsCancellationRequested)
            {
                await Task.Delay(rqQueue * (int)(Math.Abs(timeDiffInTicks) * TimeSpan.TicksPerMillisecond), token);
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
        private AccountCacheObject RequestAccountAccess(Guid accountId)
        {
            var accountCacheObject = _requestCache.GetAccount(accountId);
            if (accountCacheObject is null) accountCacheObject = new AccountCacheObject(accountId);
            accountCacheObject.RequestQueue++;
            accountCacheObject.Timestamp = DateTime.UtcNow.Ticks;
            _requestCache.SetAccount(accountCacheObject);
            return accountCacheObject;
        }

        public void UpdateAccountRequestCache(Guid accountId, HttpResponseDto resDto)
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