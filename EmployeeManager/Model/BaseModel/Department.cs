using EmployeeManager.Model.Interfaces;

namespace EmployeeManager.Model.BaseModel;

/// <summary>
/// Represents a department within the organization.
/// </summary>
public class Department : IEntity
{
    /// <summary>
    /// Gets or sets the abbreviation for the department.
    /// </summary>
    public string Abbreviation { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the department.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the department.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the department is active.
    /// </summary>
    public bool Active { get; set; }
}