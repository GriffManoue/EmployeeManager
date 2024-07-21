namespace EmployeeManager.Model;

/// <summary>
/// Represents a data transfer object for an employee.
/// </summary>
public class EmployeeDTO
{
    /// <summary>
    /// Gets or sets the unique identifier for the employee.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the employee.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the position or job title of the employee.
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the employee.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the username of the employee.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the employee's supervisor.
    /// </summary>
    public long SupervisorId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the department the employee belongs to.
    /// </summary>
    public long DepartmentId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the employee is currently active.
    /// </summary>
    public bool Active { get; set; }
}