using EmployeeManager.Model.BaseModel;

namespace EmployeeManager.Data;

/// <summary>
/// Provides methods to initialize the database with default data.
/// </summary>
public class DbInitializer
{
    /// <summary>
    /// Initializes the database with default departments and employees if they do not already exist.
    /// </summary>
    /// <param name="context">The database context to be used for data initialization.</param>
    public static void Initialize(EmployeeManagerContext context)
    {
        // Check if any departments exist, and if so, return immediately to avoid re-initialization.
        if (context.Department.Any()) return;

        // Define a list of default departments to be added to the database.
        var departments = new Department[]
        {
            new() { Name = "Human Resources", Abbreviation = "HR", Active = true },
            new() { Name = "Information Technology", Abbreviation = "IT", Active = true },
            new() { Name = "Finance", Abbreviation = "FIN", Active = false }
        };

        // Add each department to the context for saving.
        foreach (var d in departments) context.Department.Add(d);
        // Save the changes to the database.
        context.SaveChanges();

        // Check if any employees exist, and if so, return immediately to avoid re-initialization.
        if (context.Employee.Any()) return;

        // Define a list of default employees, including supervisors, to be added to the database.
        var supervisor1 = new Employee
        {
            Name = "John", Active = true, Department = departments[0], Password = "testpassword",
            PhoneNumber = "06205007447", Position = "manager", Supervisor = null, Username = "John"
        };
        var supervisor2 = new Employee
        {
            Name = "Jane", Active = true, Department = departments[1], Password = "testpassword",
            PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor1, Username = "Jane"
        };

        var employees = new Employee[]
        {
            new()
            {
                Name = "James", Active = true, Department = departments[0], Password = "testpassword",
                PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "James"
            },
            new()
            {
                Name = "Jill", Active = true, Department = departments[1], Password = "testpassword",
                PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "Jill"
            },
            new()
            {
                Name = "Jack", Active = true, Department = departments[0], Password = "testpassword",
                PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "Jack"
            },
            new()
            {
                Name = "Jenny", Active = false, Department = departments[1], Password = "testpassword",
                PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "Jenny"
            }
        };

        // Add each employee to the context for saving.
        foreach (var e in employees) context.Employee.Add(e);

        // Save the changes to the database.
        context.SaveChanges();
    }
}