using DbContextScope.UnitOfWork.Core.Repository;
using MadXchange.Common.Types;
using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Repositories
{
    public interface IApiKeySetRepository : IRepository<ApiKeySet>
    {
        public ApiKeySet GetAccount(Guid accountId);
        public void AddAccount(ApiKeySet account);
        public void RemoveAccount(Guid accountId);
        public bool Contains(Guid accountId);
    }
    public class ApiKeySetRepository : IApiKeySetRepository
    {

        private readonly Dictionary<Guid, ApiKeySet> _userDictionary;

        public ApiKeySetRepository(Dictionary<Guid, ApiKeySet> userStore) 
        {
            _userDictionary = userStore;
        }

        public ApiKeySetRepository() 
        {
            _userDictionary = new Dictionary<Guid, ApiKeySet>();
        }

        public void Add(ApiKeySet entity) => AddAccount(entity);
        
        public void AddAccount(ApiKeySet account)
        {
            _userDictionary.Add(account.Id, account);
        }

        public IQueryable<ApiKeySet> AsQueryable()
        {
            return _userDictionary.Values.AsQueryable();
        }

        public void Attach(ApiKeySet entity) => AddAccount(entity);

        public bool Contains(Guid accountId)
        {
            return _userDictionary.ContainsKey(accountId);
        }

        public void Delete(ApiKeySet entity) => RemoveAccount(entity.Id);

        public void Edit(ApiKeySet entity) => Update(entity);
       
        public ApiKeySet First(Expression<Func<ApiKeySet, bool>> predicate = null)
        {
            return _userDictionary.Values.AsQueryable().First(predicate);            
        }

        public ApiKeySet FirstOrDefault(Expression<Func<ApiKeySet, bool>> predicate = null)
        {
            return _userDictionary.Values.AsQueryable().FirstOrDefault(predicate);
        }

        public Task<ApiKeySet> FirstOrDefaultAsync(Expression<Func<ApiKeySet, bool>> predicate = null)
        {
            return Task.FromResult(_userDictionary.Values.AsQueryable().FirstOrDefault(predicate));
        }

        public IEnumerable<ApiKeySet> Get(Expression<Func<ApiKeySet, bool>> predicate = null, Func<IQueryable<ApiKeySet>, IOrderedQueryable<ApiKeySet>> orderBy = null, params Expression<Func<ApiKeySet, object>>[] includeProperties)
        {
            throw new NotSupportedException();
        }

        public ApiKeySet GetAccount(Guid accountId)
        {
            return _userDictionary.GetValueOrDefault(accountId);
        }

        public Task<IEnumerable<ApiKeySet>> GetAsync(Expression<Func<ApiKeySet, bool>> predicate = null, Func<IQueryable<ApiKeySet>, IOrderedQueryable<ApiKeySet>> orderBy = null, params Expression<Func<ApiKeySet, object>>[] includeProperties)
        {
            throw new NotSupportedException();
        }

        public ApiKeySet LastOrDefault(Expression<Func<ApiKeySet, bool>> predicate = null)
        {
            return _userDictionary.Values.AsQueryable<ApiKeySet>().LastOrDefault(predicate);
        }

        public void RemoveAccount(Guid accountId)
        {
            _userDictionary.Remove(accountId);
        }

        public ApiKeySet Single(Expression<Func<ApiKeySet, bool>> predicate)
        {
            return _userDictionary.Values.AsQueryable<ApiKeySet>().Single(predicate);
        }

        public Task<ApiKeySet> SingleAsync(Expression<Func<ApiKeySet, bool>> predicate)
        {
            return Task.FromResult(_userDictionary.Values.AsQueryable<ApiKeySet>().Single(predicate));
        }

        public ApiKeySet SingleOrDefault(Expression<Func<ApiKeySet, bool>> predicate)
        {
            return _userDictionary.Values.AsQueryable<ApiKeySet>().FirstOrDefault(predicate);
        }

        public Task<ApiKeySet> SingleOrDefaultAsync(Expression<Func<ApiKeySet, bool>> predicate)
        {
            return Task.FromResult(_userDictionary.Values.AsQueryable<ApiKeySet>().FirstOrDefault(predicate));
        }

        public void Update(ApiKeySet entityToUpdate)
        {
            _userDictionary[entityToUpdate.Id] = entityToUpdate;
        }
    }
}
