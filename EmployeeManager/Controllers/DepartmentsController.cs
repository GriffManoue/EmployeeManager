using EmployeeManager.Model;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManager.Controllers;

/// <summary>
/// Controller for managing department-related operations.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly ILogicService<Department> _logicService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentsController"/> class.
    /// </summary>
    /// <param name="logicService">The logic service for department operations.</param>
    public DepartmentsController(ILogicService<Department> logicService)
    {
        _logicService = logicService;
    }

    /// <summary>
    /// Retrieves all departments with an "active" flag set to true.
    /// </summary>
    /// <returns>A list of active departments.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
    {
        var query = await _logicService.Query(new QueryRequest("active", "true"));
        return query.ToList();
    }

    /// <summary>
    /// Retrieves a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to retrieve.</param>
    /// <returns>The requested department if found; otherwise, an internal server error.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(long id)
    {
        try
        {
            var department = await _logicService.GetByIdAsync(id);
            return department;
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    /// <summary>
    /// Queries departments based on a specified request.
    /// </summary>
    /// <param name="request">The query request parameters.</param>
    /// <returns>A list of departments that match the query request.</returns>
    [HttpGet("Query")]
    public async Task<ActionResult<IEnumerable<Department>>> QueryDepartment([FromQuery] QueryRequest request)
    {
        var query = await _logicService.Query(request);
        return query.ToList();
    }

    /// <summary>
    /// Updates a department's information.
    /// </summary>
    /// <param name="id">The ID of the department to update.</param>
    /// <param name="department">The updated department information.</param>
    /// <returns>A result indicating the success or failure of the update operation.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDepartment(long id, Department department)
    {
        if (id != department.Id) return BadRequest();

        try
        {
            await _logicService.UpdateAsync(department);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return NoContent();
    }

    /// <summary>
    /// Adds a new department.
    /// </summary>
    /// <param name="entity">The department to add.</param>
    /// <returns>The created department.</returns>
    [HttpPost]
    public async Task<ActionResult<Department>> PostDepartment(Department entity)
    {
        Department department = null;

        try
        {
            department = await _logicService.AddAsync(entity);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }

        return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
    }

    /// <summary>
    /// Deletes a department by its ID.
    /// </summary>
    /// <param name="id">The ID of the department to delete.</param>
    /// <returns>A result indicating the success or failure of the delete operation.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(long id)
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