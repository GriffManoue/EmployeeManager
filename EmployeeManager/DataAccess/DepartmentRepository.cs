

using System.Linq.Expressions;
using EmployeeManager.Data;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.DataAccess;

/// <summary>
/// Repository for managing department data operations.
/// Implements the <see cref="IRepository{TData}"/> interface for <see cref="Department"/>.
/// </summary>
public class DepartmentRepository : IRepository<Department>
{
    private readonly EmployeeManagerContext _context;
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to use for data operations.</param>
    public DepartmentRepository(EmployeeManagerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously retrieves all active departments.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of active departments.</returns>
    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        try
        {
            return await _context.Department.Where(e => e.Active == true).ToListAsync();
        }
        catch (ArgumentNullException e)
        {
            throw new DataAccessException("The list of departments is null.", e);
        }
        catch (OperationCanceledException e)
        {
            throw new DataAccessException("The operation was canceled while retrieving the departments.", e);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while retrieving the departments.", e);
        }
    }

    /// <summary>
    /// Asynchronously retrieves a department by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the department to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the department if found; otherwise, null.</returns>
    public async Task<Department> GetByIdAsync(long id)
    {
        Department department;
        try
        {
            department = await _context.Department.FindAsync(id);
            return department;
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while retrieving the department.", e);
        }
    }

    /// <summary>
    /// Asynchronously inserts a new department into the database.
    /// </summary>
    /// <param name="entity">The department to insert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted department.</returns>
    public async Task<Department> InsertAsync(Department entity)
    {
        Department department;

        try
        {
            department = (await _context.Department.AddAsync(entity)).Entity;

            return department;
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while inserting the department.", e);
        }
    }

    /// <summary>
/// Asynchronously updates an existing department in the database.
/// </summary>
/// <param name="entity">The department entity with updated values.</param>
/// <returns>A <see cref="Task"/> that represents the asynchronous operation, resulting in the updated <see cref="Department"/> object.</returns>
/// <remarks>
/// This method finds a department by its ID and updates its properties with the values from the provided <paramref name="entity"/>.
/// If the department is not found, a <see cref="DataAccessException"/> is thrown with a detailed error message.
/// </remarks>
/// <exception cref="DataAccessException">Thrown if an error occurs during the update process.</exception>
public async Task<Department> UpdateAsync(Department entity)
{
    try
    {
        var department =  await _context.Department.FindAsync(entity.Id);
        
        department.Name = entity.Name;
        department.Active = entity.Active;
        department.Abbreviation = entity.Abbreviation;
        
        return department;
    }
    catch (Exception e)
    {
        throw new DataAccessException("An error occurred while updating the department.", e);
    }
}

    /// <summary>
    /// Deletes a specified department from the database.
    /// </summary>
    /// <param name="entity">The department entity to delete.</param>
    /// <exception cref="DataAccessException">Thrown if an error occurs during the deletion process.</exception>
    public void Delete(Department entity)
    {
        try
        {
            _context.Department.Remove(entity);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while deleting the department.", e);
        }
    }

    /// <summary>
    /// Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <exception cref="DataAccessException">Thrown if an error occurs during the save operation, including concurrency and update exceptions.</exception>
    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new DataAccessException("A concurrency error occurred while saving the department.", e);
        }
        catch (DbUpdateException e)
        {
            throw new DataAccessException("An error occurred while saving the department.", e);
        }
        catch (OperationCanceledException e)
        {
            throw new DataAccessException("The operation was canceled while saving the department.", e);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while saving the department.", e);
        }
    }

    /// <summary>
    /// Queries departments based on a specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The condition to filter departments.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of departments that match the predicate.</returns>
    /// <exception cref="DataAccessException">Thrown if an error occurs during the query operation or if the predicate is null.</exception>
    public async Task<IQueryable<Department>> Query(Expression<Func<Department, bool>> predicate)
    {
        try
        {
            return await Task.FromResult(_context.Department.Where(predicate));
        }
        catch (ArgumentNullException e)
        {
            throw new DataAccessException("The predicate is null.", e);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while querying the departments.", e);
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the DepartmentRepository and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}