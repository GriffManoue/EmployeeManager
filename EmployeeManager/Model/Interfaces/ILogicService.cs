using System.Linq.Expressions;

namespace EmployeeManager.Model.Interfaces
{
    public interface ILogicService<TEntity>
    {
        Task<TEntity> GetByIdAsync(long id);
        Task<IQueryable<TEntity>> Query(QueryRequest request);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(long id);
    }

}
