using System.Linq.Expressions;

namespace EmployeeManager.Model.Interfaces
{
    public interface ILogicService<TEntity>
    {
        Task<TEntity> GetByIdAsync(long id);
        Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }

}
