using MadXchange.Common.Helpers;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using System;
using System.Threading.Tasks;
using ServiceStack.Redis;
using ServiceStack;
using System.Diagnostics;

namespace MadXchange.Common.Infrastructure
{
    public class CacheStorage<T> : IDisposable
    {
        protected readonly IRedisClient _distributedCache;
        protected readonly string _cacheAdress;

        public CacheStorage(string baseAdress, IRedisClientsManager cache)
        {
            _cacheAdress = baseAdress;
            _distributedCache = cache.GetClient();
        }

        protected string AdressString(string id) => $"{_cacheAdress}{id}";

        protected void Set(string id, T item)
        {
            
            var byteArray = Converter.ObjectToByteArray(item);
            _distributedCache.Set(AdressString(id), byteArray);
           
        }

        protected Task SetAsync(string id, T item)
        {
            var accountByteArray = Converter.ObjectToByteArray(item);
            _distributedCache.Set(AdressString(id), accountByteArray);
            
            return Task.CompletedTask;
        }

        protected T Get(string id)
        {
            var item = _distributedCache.Get<byte[]>(AdressString(id));
            var cItem = Converter.ByteArrayToObject(item);
            return (T)(cItem);
        }

        protected Task<T> GetAsync(string id)
        {
            var item = _distributedCache.Get<byte[]>(AdressString(id));
            var charAr = (T)Converter.ByteArrayToObject(item);
            return Task.FromResult(charAr);
        }

        protected void Remove(string id)
        {
            _distributedCache.Remove(AdressString(id));
        }

        protected Task RemoveAsync(string id)
        {
            return Task.FromResult(_distributedCache.Remove(AdressString(id)));
        }

        public void Dispose()
        {
            _distributedCache.Dispose();
        }
    }
}