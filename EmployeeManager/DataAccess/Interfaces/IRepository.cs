using System.Linq.Expressions;

namespace EmployeeManager.DataAccess.Interfaces;

/// <summary>
/// Defines a generic repository interface for managing data operations.
/// </summary>
/// <typeparam name="TData">The type of the data entity.</typeparam>
public interface IRepository<TData> : IDisposable
{
    /// <summary>
    /// Retrieves all entities of type TData asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of TData.</returns>
    public Task<IEnumerable<TData>> GetAllAsync();

    /// <summary>
    /// Retrieves a single entity of type TData by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type TData.</returns>
    public Task<TData> GetByIdAsync(long id);

    /// <summary>
    /// Inserts a new entity of type TData asynchronously.
    /// </summary>
    /// <param name="entity">The entity to insert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted entity of type TData.</returns>
    public Task<TData> InsertAsync(TData entity);

   /// <summary>
/// Asynchronously updates an existing entity of type <typeparamref name="TData"/>.
/// </summary>
/// <param name="entity">The entity to be updated.</param>
/// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the updated entity of type <typeparamref name="TData"/>.</returns>
public Task<TData> UpdateAsync(TData entity);
   
    /// <summary>
    /// Deletes an existing entity of type TData.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(TData entity);

    /// <summary>
    /// Saves all changes made in the context of the repository asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    public Task SaveAsync();

    /// <summary>
    /// Queries entities of type TData based on a predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The expression to filter entities.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of TData.</returns>
    public Task<IQueryable<TData>> Query(Expression<Func<TData, bool>> predicate);
}