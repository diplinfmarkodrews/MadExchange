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
        {
            _redisClient.Set(AdressString(id), Converter.ObjectToByteArray(item));            
            return Task.CompletedTask;
        }

        protected T Get(string id)
        {
            var item = _redisClient.Get<byte[]>(AdressString(id));
            var cItem = Converter.ByteArrayToObject(item);
            return (T)(cItem);
        }

        protected Task<T> GetAsync(string id)
        {
            var item = _redisClient.Get<byte[]>(AdressString(id));
            var charAr = (T)Converter.ByteArrayToObject(item);
            return Task.FromResult(charAr);
        }

        protected void Remove(string id)
        {
            _redisClient.Remove(AdressString(id));
        }

        protected Task RemoveAsync(string id)
        {
            return Task.FromResult(_redisClient.Remove(AdressString(id)));
        }

        public void Dispose()
        {
            _redisClient.Dispose();
        }
    }
}