using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;

namespace EmployeeManager.Model.Services;

/// <summary>
/// Provides logic services for employee entities, including CRUD operations and complex queries.
/// </summary>
public class EmployeeLogicService : ILogicService<Employee>
{
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<Employee> _employeeRepository;
    private readonly ILogger<EmployeeLogicService> _logger;
    private readonly IPasswordService<Employee> _passwordService;
    private readonly Dictionary<string, Func<string, Task<IEnumerable<Employee>>>> _queryActions = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeLogicService"/> class.
    /// </summary>
    /// <param name="logger">The logger for logging errors and information.</param>
    /// <param name="employeeRepository">The repository for accessing employee data.</param>
    /// <param name="departmentRepository">The repository for accessing department data.</param>
    /// <param name="passwordService">The service for handling password hashing and validation.</param>
    public EmployeeLogicService(ILogger<EmployeeLogicService> logger, IRepository<Employee> employeeRepository,
        IRepository<Department> departmentRepository, IPasswordService<Employee> passwordService)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
        _passwordService = passwordService;
        _departmentRepository = departmentRepository;

        // Initialize query actions for various employee attributes.
        _queryActions.Add("id",
            async x => new List<Employee> { await _employeeRepository.GetByIdAsync(long.Parse(x)) });
        _queryActions.Add("name", async x => await _employeeRepository.Query(e => e.Name.Contains(x)));
        _queryActions.Add("department", async x => await _employeeRepository.Query(e => e.Department.Name.Contains(x)));
        _queryActions.Add("active", async x => await _employeeRepository.Query(e => e.Active == bool.Parse(x)));
        _queryActions.Add("position", async x => await _employeeRepository.Query(e => e.Position.Contains(x)));
        _queryActions.Add("phone", async x => await _employeeRepository.Query(e => e.PhoneNumber.Contains(x)));
        _queryActions.Add("username", async x => await _employeeRepository.Query(e => e.Username.Contains(x)));
        _queryActions.Add("supervisor",
            async x => await _employeeRepository.Query(e => e.Supervisor != null && e.Supervisor.Name.Contains(x)));
    }

    /// <summary>
    /// Adds a new employee asynchronously after hashing their password and validating their department.
    /// </summary>
    /// <param name="entity">The employee to add.</param>
    /// <returns>The added employee entity, or null if the operation fails.</returns>
    /// <exception cref="DepartmentNotFoundException">Thrown if the specified department does not exist.</exception>
    /// <exception cref="EmployeeAlreadyExistsException">Thrown if an employee with the given ID already exists.</exception>
    public async Task<Employee> AddAsync(Employee entity)
    {
        Employee? employee = null;

        try
        {
            employee = await _employeeRepository.GetByIdAsync(entity.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        if (employee == null)
        {
            entity.Password = _passwordService.HashPassword(entity, entity.Password);
            try
            {
                var department = await _departmentRepository.GetByIdAsync(entity.Department.Id);

                if (department == null)
                {
                    throw new DepartmentNotFoundException("The department with the given id was not found.",
                        entity.Department.Id);
                }

                await _employeeRepository.InsertAsync(entity);
                await _employeeRepository.SaveAsync();
                return entity;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new DataAccessException("An error occurred while inserting the employee.", e);
            }
        }
        else
        {
            _logger.LogError("An employee with the given id already exists.");
            throw new EmployeeAlreadyExistsException("An employee with the given id already exists.", entity.Id);
        }
        
    }

    /// <summary>
    /// Deletes an employee by their ID asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the employee to delete.</param>
    /// <exception cref="EmployeeNotFoundException">Thrown if the employee with the given ID is not found.</exception>
    public async Task DeleteAsync(long id)
    {
        try
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee != null)
            {
                _employeeRepository.Delete(employee);
                await _employeeRepository.SaveAsync();
            }
            else
            {
                throw new EmployeeNotFoundException("An employee with the given id was not found.", id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new DataAccessException("An error occurred while deleting the employee.", e);
        }
    }

    /// <summary>
    /// Retrieves an employee by their unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the employee.</param>
    /// <returns>The employee entity if found; otherwise, null. Throws an EmployeeNotFoundException if the employee is not found.</returns>
    /// <exception cref="EmployeeNotFoundException">Thrown when an employee with the specified ID cannot be found.</exception>
    public async Task<Employee> GetByIdAsync(long id)
    {
        Employee employee = null;
        try
        {
            employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
            {
                throw new EmployeeNotFoundException("An employee with the given id was not found.", id);
            }

            return employee;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new DataAccessException("An error occurred while retrieving the employee.", e);
        }

     
    }

    /// <summary>
    /// Executes a query on the employee entities based on a specified attribute and value.
    /// </summary>
    /// <param name="request">The query request containing the attribute and value to query by.</param>
    /// <returns>An IQueryable of Employee entities that match the query criteria.</returns>
    /// <exception cref="InvalidAttributeException">Thrown when the specified attribute is not valid for querying.</exception>
    public async Task<IQueryable<Employee>> Query(QueryRequest request)
    {
        var attribute = request.Attribute.ToLower();
        var value = request.Value.ToLower();

        if (!_queryActions.ContainsKey(attribute))
        {
            _logger.LogError("The attribute is not valid.");
            throw new InvalidAttributeException("The attribute is not valid.", attribute);
        }

        return (await _queryActions[attribute](value)).AsQueryable();
    }

    /// <summary>
    /// Updates an existing employee entity asynchronously after validating the department and hashing the password if it has changed.
    /// </summary>
    /// <param name="entity">The employee entity to update.</param>
    /// <returns>The updated employee entity, or null if the employee does not exist. Throws a DepartmentNotFoundException if the department does not exist.</returns>
    /// <exception cref="EmployeeNotFoundException">Thrown if the employee with the given ID is not found.</exception>
    /// <exception cref="DepartmentNotFoundException">Thrown if the department with the given ID is not found.</exception>
    public async Task<Employee> UpdateAsync(Employee entity)
    {
        Employee employee = null;

        try
        {
            employee = await _employeeRepository.GetByIdAsync(entity.Id);
            var department = await _departmentRepository.GetByIdAsync(entity.Department.Id);
            var supervisor = await _employeeRepository.GetByIdAsync(entity.Supervisor?.Id ?? 0);

            if (employee != null)
            {
                if (department == null)
                {
                    throw new DepartmentNotFoundException("The department with the given id was not found.",
                        entity.Department.Id);
                }

                if (supervisor == null)
                {
                    throw new EmployeeNotFoundException("The supervisor with the given id was not found.",
                        entity.Supervisor?.Id ?? 0);
                }

                if (entity.Password != employee.Password)
                    entity.Password = _passwordService.HashPassword(entity, entity.Password);

                await _employeeRepository.UpdateAsync(entity);
                await _employeeRepository.SaveAsync();
                return await _employeeRepository.GetByIdAsync(entity.Id);
            }
            else
            {
                _logger.LogError("An employee with the given id was not found.");
                throw new EmployeeNotFoundException("An employee with the given id was not found.", entity.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new DataAccessException("An error occurred while updating the employee.", e);
        }
    }

}