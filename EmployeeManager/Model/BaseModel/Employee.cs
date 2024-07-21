namespace EmployeeManager.Model.BaseModel;

/// <summary>
/// Represents an employee within the organization.
/// </summary>
public class Employee : Entity
{
    /// <summary>
    /// Gets or sets the position of the employee within the organization.
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the employee.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the username for the employee's account.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password for the employee's account.
    /// Note: Storing passwords in plain text is highly discouraged due to security concerns.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the supervisor of the employee. Can be null if the employee does not have a supervisor.
    /// </summary>
    public Entity? Supervisor { get; set; }

    /// <summary>
    /// Gets or sets the department the employee belongs to.
    /// </summary>
    public Department Department { get; set; }
}