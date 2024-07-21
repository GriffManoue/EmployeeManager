using EmployeeManager.Model.BaseModel;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.Data;

/// <summary>
/// Represents the database context for the Employee Manager application.
/// This class is responsible for configuring the model and its relationships
/// and acts as a bridge between the application's data models and the database.
/// </summary>
public class EmployeeManagerContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeManagerContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public EmployeeManagerContext(DbContextOptions<EmployeeManagerContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the DbSet for Employees.
    /// </summary>
    public DbSet<Employee> Employee { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for Departments.
    /// </summary>
    public DbSet<Department> Department { get; set; }
}