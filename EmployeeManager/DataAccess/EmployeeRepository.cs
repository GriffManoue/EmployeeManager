using System.Linq.Expressions;
using EmployeeManager.Data;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.DataAccess;

/// <summary>
/// Repository for managing employee data operations.
/// Implements the <see cref="IRepository{TData}"/> interface for <see cref="Employee"/>.
/// </summary>
public class EmployeeRepository : IRepository<Employee>
{
    private readonly EmployeeManagerContext _context;
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context to use for data operations.</param>
    public EmployeeRepository(EmployeeManagerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Asynchronously retrieves all active employees.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of active employees.</returns>
    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        try
        {
            return await _context.Employee.Where(e => e.Active == true).ToListAsync();
        }
        catch (ArgumentNullException e)
        {
            throw new DataAccessException("The list of employees is null.", e);
        }
        catch (OperationCanceledException e)
        {
            throw new DataAccessException("The operation was canceled while retrieving the employees.", e);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while retrieving the employees.", e);
        }
    }

    /// <summary>
    /// Asynchronously retrieves an employee by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the employee to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the employee if found; otherwise, null.</returns>
    public async Task<Employee> GetByIdAsync(long id)
    {
        Employee employee;
        try
        {
            employee = await _context.Employee.FindAsync(id);
            return employee;
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while retrieving the employee.", e);
        }
    }

    /// <summary>
    /// Asynchronously inserts a new employee into the database.
    /// </summary>
    /// <param name="entity">The employee to insert.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted employee.</returns>
    public async Task<Employee> InsertAsync(Employee entity)
    {
        Employee employee;
        try
        {
            employee = (await _context.Employee.AddAsync(entity)).Entity;
            return employee;
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while inserting the employee.", e);
        }
    }

   /// <summary>
/// Updates an existing employee asynchronously.
/// </summary>
/// <param name="entity">The employee entity with updated values.</param>
/// <returns>A <see cref="Task"/> that represents the asynchronous operation, resulting in the updated <see cref="Employee"/> object.</returns>
/// <remarks>
/// This method finds an employee by ID and updates its properties with the values from the provided <paramref name="entity"/>.
/// If the employee is not found, no action is taken. The method encapsulates the update operation within a try-catch block,
/// throwing a <see cref="DataAccessException"/> with a detailed error message if an exception occurs during the update process.
/// </remarks>
public async Task<Employee> UpdateAsync(Employee entity)
{
    try
    {
        var employee = await _context.Employee.FindAsync(entity.Id);
        var department = await _context.Department.FindAsync(entity.Department.Id);
        var supervisor = await _context.Employee.FindAsync(entity.Supervisor.Id);

        employee.Department = department;
        employee.Name = entity.Name;
        employee.Active = entity.Active;
        employee.Position = entity.Position;
        employee.Password = entity.Password;
        employee.Supervisor = supervisor;
        employee.Username = entity.Username;
        employee.PhoneNumber = entity.PhoneNumber;
        
    }
    catch (Exception e)
    {
        throw new DataAccessException("An error occurred while updating the employee.", e);
    }

    return entity;
}

    /// <summary>
    /// Deletes a specified employee from the database.
    /// </summary>
    /// <param name="entity">The employee entity to delete.</param>
    /// <exception cref="DataAccessException">Thrown if an error occurs during the deletion process.</exception>
    public void Delete(Employee entity)
    {
        try
        {
            _context.Employee.Remove(entity);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while deleting the employee.", e);
        }
    }


    /// <summary>
    /// Saves all changes made in the context to the database asynchronously.
    /// </summary>
    /// <remarks>
    /// This method attempts to save all changes made in the context to the database asynchronously.
    /// It handles several types of exceptions that might occur during the save operation, including
    /// concurrency conflicts, update issues, operation cancellations, and other exceptions, by throwing
    /// a <see cref="DataAccessException"/> with a specific message for each case.
    /// </remarks>
    /// <exception cref="DataAccessException">Thrown when an error occurs during the save operation.</exception>
    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e)
        {
            throw new DataAccessException("A concurrency update error occurred while saving the changes.", e);
        }
        catch (DbUpdateException e)
        {
            throw new DataAccessException("An update error occurred while saving the changes.", e);
        }
        catch (OperationCanceledException e)
        {
            throw new DataAccessException("The operation was canceled while saving the changes.", e);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while saving the changes.", e);
        }
    }

    /// <summary>
    /// Queries employees based on a specified predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The condition to filter employees.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an IQueryable of employees that match the predicate.</returns>
    /// <exception cref="DataAccessException">Thrown when an error occurs during the query operation or if the predicate is null.</exception>
    public async Task<IQueryable<Employee>> Query(Expression<Func<Employee, bool>> predicate)
    {
        try
        {
            return await Task.FromResult(_context.Employee.Where(predicate));
        }
        catch (ArgumentNullException e)
        {
            throw new DataAccessException("The predicate is null.", e);
        }
        catch (Exception e)
        {
            throw new DataAccessException("An error occurred while querying the employees.", e);
        }
    }

    /// <summary>
    /// Disposes the context and releases all resources used by the EmployeeRepository.
    /// </summary>
    /// <remarks>
    /// This method disposes the DbContext (if disposing is true) and marks the repository as disposed
    /// to prevent further use. It is designed to be called both from the IDisposable.Dispose method and
    /// from a finalizer, to allow for proper resource cleanup.
    /// </remarks>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the context and releases all resources used by the EmployeeRepository.
    /// </summary>
    /// <param name="disposing">Indicates whether the method has been called from the Dispose() method and not from a finalizer.</param>
    /// <remarks>
    /// If disposing is true, the method disposes the DbContext. This method is protected and virtual to allow derived classes
    /// to override the disposal logic.
    /// </remarks>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing) _context.Dispose();
            _disposedValue = true;
        }
    }
}