using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;

namespace EmployeeManager.Model.Services;

/// <summary>
/// Provides logic services for department entities, including CRUD operations and custom queries.
/// </summary>
public class DepartmentLogicService : ILogicService<Department>
{
    private readonly ILogger<DepartmentLogicService> _logger;
    private readonly IRepository<Department> _repository;
    private readonly Dictionary<string, Func<string, Task<IEnumerable<Department>>>> _queryActions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentLogicService"/> class.
    /// </summary>
    /// <param name="logger">The logger to use for logging information or errors.</param>
    /// <param name="repository">The repository providing data access for departments.</param>
    public DepartmentLogicService(ILogger<DepartmentLogicService> logger, IRepository<Department> repository)
    {
        _logger = logger;
        _repository = repository;

        // Initialize query actions for different department attributes
        _queryActions.Add("id", async x => new List<Department> { await _repository.GetByIdAsync(long.Parse(x)) });
        _queryActions.Add("name", async x => await _repository.Query(d => d.Name.Contains(x)));
        _queryActions.Add("abbreviation", async x => await _repository.Query(d => d.Abbreviation.Contains(x)));
        _queryActions.Add("active", async x => await _repository.Query(d => d.Active.ToString().Contains(x)));
    }

    /// <summary>
    /// Adds a new department asynchronously.
    /// </summary>
    /// <param name="entity">The department to add.</param>
    /// <returns>The added department entity, or null if the operation fails.</returns>
    public async Task<Department> AddAsync(Department entity)
    {
        Department department = null;

        try
        {
            department = await _repository.GetByIdAsync(entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        if (department == null)
        {
            try
            {
                await _repository.InsertAsync(entity);
                await _repository.SaveAsync();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
        else
        {
            _logger.LogError("The department with the given id already exists.", entity.Id);
            throw new DepartmentAlreadyExistsException("The department with the given id already exists.", entity.Id);
        }

        return null;
    }

    /// <summary>
    /// Deletes a department by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the department to delete.</param>
    public async Task DeleteAsync(long id)
    {
        try
        {
            var department = await _repository.GetByIdAsync(id);

            if (department != null)
            {
                _repository.Delete(department);
                await _repository.SaveAsync();
            }
            else
            {
                _logger.LogError("The department with the given id was not found.", id);
                throw new DepartmentNotFoundException("The department with the given id was not found.", id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a department by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the department to retrieve.</param>
    /// <returns>The found department entity, or null if not found.</returns>
    public async Task<Department> GetByIdAsync(long id)
    {
        Department department = null;

        try
        {
            department = await _repository.GetByIdAsync(id);

            if (department == null)
            {
                _logger.LogError("The department with the given id was not found.", id);
                throw new DepartmentNotFoundException("The department with the given id was not found.", id);
            }
        }
        catch (DepartmentNotFoundException e)
        {
            _logger.LogError(e.Message, id);
        }

        return department;
    }

    /// <summary>
    /// Executes a query on department entities based on a specified attribute and value.
    /// </summary>
    /// <param name="request">The query request containing the attribute and value to query by.</param>
    /// <returns>An IQueryable of Department entities that match the query criteria.</returns>
    /// <exception cref="InvalidAttributeException">Thrown when the specified attribute is not valid for querying.</exception>
    public async Task<IQueryable<Department>> Query(QueryRequest request)
    {
        var attribute = request.Attribute.ToLower();
        var value = request.Value.ToLower();

        if (!_queryActions.ContainsKey(attribute))
        {
            _logger.LogError("The attribute is not valid.", attribute);
            throw new InvalidAttributeException("The attribute is not valid.", attribute);
        }

        var result = await _queryActions[attribute](value);

        return result.AsQueryable();
    }

    /// <summary>
    /// Updates an existing department entity asynchronously.
    /// </summary>
    /// <param name="entity">The department entity to update.</param>
    /// <returns>The updated department entity, or null if the department does not exist.</returns>
    /// <exception cref="DepartmentNotFoundException">Thrown when the department to update is not found.</exception>
    public async Task<Department> UpdateAsync(Department entity)
    {
        Department department = null;

        try
        {
            department = await _repository.GetByIdAsync(entity.Id);

            if (department != null)
            {
                await _repository.UpdateAsync(entity);
                await _repository.SaveAsync();
                return await _repository.GetByIdAsync(entity.Id);
            }
            else
            {
                _logger.LogError("The department with the given id was not found.", entity.Id);
                throw new DepartmentNotFoundException("The department with the given id was not found.", entity.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        return null;
    }
}