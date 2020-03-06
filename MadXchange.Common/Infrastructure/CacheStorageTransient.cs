using MadXchange.Common.Helpers;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Common.Infrastructure
{
    public class CacheStorageTransient<T> : IDisposable
    {
        protected readonly IRedisClient _redisClient;
        protected readonly string _cacheAdress;

        public CacheStorageTransient(string baseAdress, IRedisClientsManager cache)
        {
            _cacheAdress = baseAdress;
            _redisClient = cache.GetClient();           
        }

        protected IDisposable AquireLock(string id)         
            => _redisClient.AcquireLock(id);
        
        protected string AdressString(string id) 
            => $"{_cacheAdress}{id}";

        protected void Set(string id, T item)          
            => _redisClient.Set(AdressString(id), Converter.ObjectToByteArray(item));           
        
        protected Task SetAsync(string id, T item)          
            => Task.FromResult(_redisClient.Set(AdressString(id), Converter.ObjectToByteArray(item)));        

        protected T Get(string id)
            => (T)(Converter.ByteArrayToObject(_redisClient.Get<byte[]>(AdressString(id))));        

        protected Task<T> GetAsync(string id)
            => Task.FromResult((T)Converter.ByteArrayToObject(_redisClient.Get<byte[]>(AdressString(id))));
        
        protected void Remove(string id)
            => _redisClient.Remove(AdressString(id));

        protected Task RemoveAsync(string id)
            => Task.FromResult(_redisClient.Remove(AdressString(id)));

        public void Dispose()
        {
            _redisClient.Dispose();
        }
    }
}