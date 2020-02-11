using MadXchange.Common.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Common.Infrastructure
{
    public class CacheStorage<T> : IDisposable
    {
        protected readonly IDistributedCache _distributedCache;
        protected readonly string _cacheAdress;

        public CacheStorage(string baseAdress, IDistributedCache cache)
        {
            _cacheAdress = baseAdress;
            _distributedCache = cache;
        }

        protected string AdressString(string id) => $"{_cacheAdress}{id}";

        protected void Set(string id, T item)
        {
            var accountByteArray = Converter.ObjectToByteArray(item);
            _distributedCache.Set(AdressString(id), accountByteArray);
        }

        protected async Task SetAsync(string id, T item)
        {
            var accountByteArray = Converter.ObjectToByteArray(item);
            await _distributedCache.SetAsync(AdressString(id), accountByteArray);
        }

        protected T Get(string id)
        {
            var item = _distributedCache.Get(AdressString(id));
            return (T)Converter.ByteArrayToObject(item);
        }

        protected async Task<T> GetAsync(string id)
        {
            var item = await _distributedCache.GetAsync(AdressString(id));
            return (T)Converter.ByteArrayToObject(item);
        }

        protected void Remove(string id)
        {
            _distributedCache.Remove(AdressString(id));
        }

        protected async Task RemoveAsync(string id)
        {
            await _distributedCache.RemoveAsync(AdressString(id));
        }

        public void Dispose()
        {
            if (_distributedCache is RedisCache)
            {
                (_distributedCache as RedisCache).Dispose();
            }
        }
    }
}