using AutoMapper;
using EmployeeManager.Model;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers;

/// <summary>
/// Controller for managing employee-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly ILogicService<Employee> _logicService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeesController"/> class.
    /// </summary>
    /// <param name="logicService">The logic service for employee operations.</param>
    /// <param name="mapper">The AutoMapper instance for mapping domain models to DTOs.</param>
    public EmployeesController(ILogicService<Employee> logicService, IMapper mapper)
    {
        _logicService = logicService;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all active employees.
    /// </summary>
    /// <returns>A list of active employee DTOs.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
    {
        var query = await _logicService.Query(new QueryRequest("active", "true"));
        var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(query);
        return Ok(employeeDTOs);
    }

    /// <summary>
    /// Retrieves a specific employee by ID.
    /// </summary>
    /// <param name="id">The ID of the employee to retrieve.</param>
    /// <returns>The requested employee DTO if found; otherwise, an internal server error.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDTO>> GetEmployee(long id)
    {
        try
        {
            var employee = await _logicService.GetByIdAsync(id);
            return _mapper.Map<EmployeeDTO>(employee);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Queries employees based on a specified request.
    /// </summary>
    /// <param name="request">The query request parameters.</param>
    /// <returns>A list of employee DTOs that match the query request.</returns>
    [HttpGet("Query")]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> QueryEmployee([FromQuery] QueryRequest request)
    {
        var query = await _logicService.Query(request);
        var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(query);
        return Ok(employeeDTOs);
    }

    /// <summary>
    /// Updates a specific employee's information.
    /// </summary>
    /// <param name="id">The ID of the employee to update.</param>
    /// <param name="employee">The updated employee information.</param>
    /// <returns>A result indicating the success or failure of the update operation.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(long id, Employee employee)
    {
        if (id != employee.Id) return BadRequest();

        try
        {
            await _logicService.UpdateAsync(employee);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return NoContent();
    }

    /// <summary>
    /// Adds a new employee.
    /// </summary>
    /// <param name="entity">The employee to add.</param>
    /// <returns>The created employee DTO.</returns>
    [HttpPost]
    public async Task<ActionResult<EmployeeDTO>> PostEmployee(Employee entity)
    {
        Employee employee = null;

        try
        {
            employee = await _logicService.AddAsync(entity);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
    }

    /// <summary>
    /// Deletes a specific employee by ID.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    /// <returns>A result indicating the success or failure of the delete operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(long id)
    {
        try
        {
            await _logicService.DeleteAsync(id);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return NoContent();
    }
}