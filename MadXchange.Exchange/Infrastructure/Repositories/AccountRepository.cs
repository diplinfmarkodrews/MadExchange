using DbContextScope.UnitOfWork.Core.Repository;
using MadXchange.Exchange.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MadXchange.Exchange.Infrastructure.Repositories
{
    public interface IAccountRepository : IRepository<IUserAccount>
    {
        public IUserAccount GetAccount(Guid accountId);
        public void AddAccount(IUserAccount account);
        public void RemoveAccount(Guid accountId);
        public bool Contains(Guid accountId);
    }
    public class AccountRepository : IAccountRepository
    {

        private readonly Dictionary<Guid, IUserAccount> _userDictionary;

        public AccountRepository(Dictionary<Guid, IUserAccount> userStore) 
        {
            _userDictionary = userStore;
        }

        public void Add(IUserAccount entity) => AddAccount(entity);
        
        public void AddAccount(IUserAccount account)
        {
            _userDictionary.Add(account.Id, account);
        }

        public IQueryable<IUserAccount> AsQueryable()
        {
            return _userDictionary.Values.AsQueryable();
        }

        public void Attach(IUserAccount entity) => AddAccount(entity);

        public bool Contains(Guid accountId)
        {
            return _userDictionary.ContainsKey(accountId);
        }

        public void Delete(IUserAccount entity) => RemoveAccount(entity.Id);

        public void Edit(IUserAccount entity) => Update(entity);
       
        public IUserAccount First(Expression<Func<IUserAccount, bool>> predicate = null)
        {
            return _userDictionary.Values.AsQueryable().First(predicate);            
        }

        public IUserAccount FirstOrDefault(Expression<Func<IUserAccount, bool>> predicate = null)
        {
            return _userDictionary.Values.AsQueryable().FirstOrDefault(predicate);
        }

        public Task<IUserAccount> FirstOrDefaultAsync(Expression<Func<IUserAccount, bool>> predicate = null)
        {
            return Task.FromResult(_userDictionary.Values.AsQueryable().FirstOrDefault(predicate));
        }

        public IEnumerable<IUserAccount> Get(Expression<Func<IUserAccount, bool>> predicate = null, Func<IQueryable<IUserAccount>, IOrderedQueryable<IUserAccount>> orderBy = null, params Expression<Func<IUserAccount, object>>[] includeProperties)
        {
            throw new NotSupportedException();
        }

        public IUserAccount GetAccount(Guid accountId)
        {
            return _userDictionary.GetValueOrDefault(accountId);
        }

        public Task<IEnumerable<IUserAccount>> GetAsync(Expression<Func<IUserAccount, bool>> predicate = null, Func<IQueryable<IUserAccount>, IOrderedQueryable<IUserAccount>> orderBy = null, params Expression<Func<IUserAccount, object>>[] includeProperties)
        {
            throw new NotSupportedException();
        }

        public IUserAccount LastOrDefault(Expression<Func<IUserAccount, bool>> predicate = null)
        {
            return _userDictionary.Values.AsQueryable<IUserAccount>().LastOrDefault(predicate);
        }

        public void RemoveAccount(Guid accountId)
        {
            _userDictionary.Remove(accountId);
        }

        public IUserAccount Single(Expression<Func<IUserAccount, bool>> predicate)
        {
            return _userDictionary.Values.AsQueryable<IUserAccount>().Single(predicate);
        }

        public Task<IUserAccount> SingleAsync(Expression<Func<IUserAccount, bool>> predicate)
        {
            return Task.FromResult(_userDictionary.Values.AsQueryable<IUserAccount>().Single(predicate));
        }

        public IUserAccount SingleOrDefault(Expression<Func<IUserAccount, bool>> predicate)
        {
            return _userDictionary.Values.AsQueryable<IUserAccount>().FirstOrDefault(predicate);
        }

        public Task<IUserAccount> SingleOrDefaultAsync(Expression<Func<IUserAccount, bool>> predicate)
        {
            return Task.FromResult(_userDictionary.Values.AsQueryable<IUserAccount>().FirstOrDefault(predicate));
        }

        public void Update(IUserAccount entityToUpdate)
        {
            _userDictionary[entityToUpdate.Id] = entityToUpdate;
        }
    }
}
