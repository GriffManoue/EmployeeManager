using System.Linq.Expressions;

namespace EmployeeManager.DataAccess.Interfaces
{
    public interface IRepository<TData> : IDisposable
    {
        public Task<IEnumerable<TData>> GetAllAsync();
        public Task<TData> GetByIdAsync(long id);
        public Task<TData> InsertAsync(TData entity);
        public TData Update(TData entity);
        public void Delete(TData entity);
        public Task SaveAsync();
        public Task<IQueryable<TData>> Query(Expression<Func<TData, bool>> predicate);
    }
}
