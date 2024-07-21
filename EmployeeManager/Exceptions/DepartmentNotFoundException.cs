namespace EmployeeManager.Exceptions;

/// <summary>
/// Represents errors that occur when a specified department cannot be found.
/// </summary>
public class DepartmentNotFoundException : Exception
{
    /// <summary>
    /// Gets the identifier of the department that caused the exception.
    /// </summary>
    public long DepartmentId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DepartmentNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentNotFoundException"/> class with a specified error message and department identifier.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="id">The identifier of the department that caused the exception.</param>
    public DepartmentNotFoundException(string message, long id) : base(message)
    {
        DepartmentId = id;
    }
}