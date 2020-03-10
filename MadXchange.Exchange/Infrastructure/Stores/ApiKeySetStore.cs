using MadXchange.Exchange.Domain.Models;
using MadXchange.Exchange.Domain.Types;
using MadXchange.Exchange.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Stores
{
    public interface IApiKeySetStore //: IRepository<ApiKeySet>
    {
        public ApiKeySet GetAccount(Guid accountId);

        public Xchange XchangeOf(Guid id);

        public void AddAccount(ApiKeySet account);

        public void RemoveAccount(Guid accountId);

        public bool Contains(Guid accountId);
        public ApiKeySet Get(Xchange exchange);

        //events to signal accounts have been added or removed
    }

    public class ApiKeySetStore : IApiKeySetStore
    {
        private readonly Dictionary<Guid, ApiKeySet> _userDictionary;// = new Dictionary<Guid, ApiKeySet>();

        public ApiKeySetStore(Dictionary<Guid, ApiKeySet> userStore)
             => _userDictionary = userStore;       

        public void AddAccount(ApiKeySet account)
             => _userDictionary.Add(account.Id, account);

        public IQueryable<ApiKeySet> AsQueryable()
             => _userDictionary.Values.AsQueryable();

        public void Attach(ApiKeySet entity)
             => AddAccount(entity);

        public bool Contains(Guid accountId)
             => _userDictionary.ContainsKey(accountId);

        public void Delete(ApiKeySet entity)
             => RemoveAccount(entity.Id);

        public void Edit(ApiKeySet entity)
             => Update(entity);

        public ApiKeySet First(Expression<Func<ApiKeySet, bool>> predicate = null)
             => _userDictionary.Values.AsQueryable().First(predicate);            

        public ApiKeySet FirstOrDefault(Expression<Func<ApiKeySet, bool>> predicate = null)        
             => _userDictionary.Values.AsQueryable().FirstOrDefault(predicate);

        public Task<ApiKeySet> FirstOrDefaultAsync(Expression<Func<ApiKeySet, bool>> predicate = null)
             => Task.FromResult(_userDictionary.Values.AsQueryable().FirstOrDefault(predicate));

        public ApiKeySet GetAccount(Guid accountId)
             => _userDictionary.GetValueOrDefault(accountId);

        public ApiKeySet LastOrDefault(Expression<Func<ApiKeySet, bool>> predicate = null)
             => _userDictionary.Values.AsQueryable<ApiKeySet>().LastOrDefault(predicate);

        public void RemoveAccount(Guid accountId)
             => _userDictionary.Remove(accountId);

        public ApiKeySet Single(Expression<Func<ApiKeySet, bool>> predicate)
             => _userDictionary.Values.AsQueryable<ApiKeySet>().Single(predicate);

        public Task<ApiKeySet> SingleAsync(Expression<Func<ApiKeySet, bool>> predicate)
             => Task.FromResult(_userDictionary.Values.AsQueryable<ApiKeySet>().Single(predicate));

        public ApiKeySet SingleOrDefault(Expression<Func<ApiKeySet, bool>> predicate)
             => _userDictionary.Values.AsQueryable<ApiKeySet>().FirstOrDefault(predicate);

        public Task<ApiKeySet> SingleOrDefaultAsync(Expression<Func<ApiKeySet, bool>> predicate) 
             => Task.FromResult(_userDictionary.Values.AsQueryable<ApiKeySet>().FirstOrDefault(predicate));

        public void Update(ApiKeySet entityToUpdate)
             => _userDictionary[entityToUpdate.Id] = entityToUpdate;

        public Xchange XchangeOf(Guid id)
             => _userDictionary[id].Exchange;

        public ApiKeySet Get(Xchange exchange)
            => _userDictionary.Values.FirstOrDefault(k => k.Exchange == exchange);
    }
}