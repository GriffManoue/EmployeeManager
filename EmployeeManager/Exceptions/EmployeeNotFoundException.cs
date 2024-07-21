namespace EmployeeManager.Exceptions;

/// <summary>
/// Represents errors that occur when a specified employee cannot be found.
/// </summary>
public class EmployeeNotFoundException : Exception
{
    /// <summary>
    /// Gets the identifier of the employee that caused the exception.
    /// </summary>
    public long EmployeeId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EmployeeNotFoundException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeNotFoundException"/> class with a specified error message and employee identifier.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="id">The identifier of the employee that caused the exception.</param>
    public EmployeeNotFoundException(string message, long id) : base(message)
    {
        EmployeeId = id;
    }
}