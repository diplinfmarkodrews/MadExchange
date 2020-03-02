using MadXchange.Common.Helpers;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

namespace MadXchange.Common.Infrastructure
{
    public class CacheStorage<T> : IDisposable
    {
        protected readonly IRedisClientsManager _redisClientManager;
        protected readonly string _cacheAdress;

        public CacheStorage(string baseAdress, IRedisClientsManager cacheManager)
        {
            _cacheAdress = baseAdress;
            _redisClientManager = cacheManager;
            
        }

        protected IDisposable AquireLock(string id)         
            => _redisClientManager.GetClient().AcquireLock(id);
        


        protected string AdressString(string id) 
            => $"{_cacheAdress}{id}";

        protected void Set(string id, T item)          
            => _redisClientManager.GetClient().Set(AdressString(id), Converter.ObjectToByteArray(item));


        protected Task SetAsync(string id, T item)
            => Task.FromResult(_redisClientManager.GetClient().Set(AdressString(id), Converter.ObjectToByteArray(item))); 
                    

        protected T Get(string id)                    
            => (T)Converter.ByteArrayToObject(_redisClientManager.GetReadOnlyClient().Get<byte[]>(AdressString(id)));
        
        
        protected Task<T> GetAsync(string id)                    
            => Task.FromResult((T)Converter.ByteArrayToObject(_redisClientManager.GetReadOnlyClient().Get<byte[]>(AdressString(id))));
        

        protected void Remove(string id)        
            => _redisClientManager.GetClient().Remove(AdressString(id));
        

        protected Task RemoveAsync(string id)
            => Task.FromResult(_redisClientManager.GetClient().Remove(AdressString(id)));
        

        public void Dispose()
        {
           
        }
    }
}