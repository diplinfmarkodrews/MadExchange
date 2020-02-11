namespace MadXchange.Common.Infrastructure
{
    //public class EfRepository<T> : IAsyncRepository<T> where T is class, IIdentifiable<Guid>
    //{
    //    #region Fields

    //    protected DataDbContext Context;

    //    #endregion

    //    public EfRepository(DataDbContext context)
    //    {
    //        Context = context;
    //    }

    //    #region Public Methods

    //    public async Task<T> GetById(Guid id) => await Context.Set<T>().FindAsync(id);

    //    public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
    //        => Context.Set<T>().FirstOrDefaultAsync(predicate);

    //    public async Task Add(T entity)
    //    {
    //        // await Context.AddAsync(entity);
    //        await Context.Set<T>().AddAsync(entity);
    //        await Context.SaveChangesAsync();
    //    }

    //    public Task Update(T entity)
    //    {
    //        // In case AsNoTracking is used
    //        Context.Entry(entity).State = EntityState.Modified;
    //        return Context.SaveChangesAsync();
    //    }

    //    public Task Remove(T entity)
    //    {
    //        Context.Set<T>().Remove(entity);
    //        return Context.SaveChangesAsync();
    //    }

    //    public async Task<IEnumerable<T>> GetAll()
    //    {
    //        return await Context.Set<T>().ToListAsync();
    //    }

    //    public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
    //    {
    //        return await Context.Set<T>().Where(predicate).ToListAsync();
    //    }

    //    public Task<int> CountAll() => Context.Set<T>().CountAsync();

    //    public Task<int> CountWhere(Expression<Func<T, bool>> predicate)
    //        => Context.Set<T>().CountAsync(predicate);

    //    #endregion

    //}
}