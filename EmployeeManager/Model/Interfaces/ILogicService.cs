namespace EmployeeManager.Model.Interfaces;

/// <summary>
/// Defines a generic interface for logic services in the EmployeeManager project.
/// Provides a set of CRUD operations for entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity this service will manage.</typeparam>
public interface ILogicService<TEntity>
{
    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity found.</returns>
    Task<TEntity> GetByIdAsync(long id);

    /// <summary>
    /// Queries entities based on a specified query request asynchronously.
    /// </summary>
    /// <param name="request">The query request containing query parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of entities.</returns>
    Task<IQueryable<TEntity>> Query(QueryRequest request);

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity.</returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(long id);
}