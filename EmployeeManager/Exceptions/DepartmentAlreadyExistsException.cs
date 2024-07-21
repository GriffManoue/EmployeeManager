namespace EmployeeManager.Exceptions;

/// <summary>
/// Represents errors that occur when attempting to add a department that already exists.
/// </summary>
public class DepartmentAlreadyExistsException : Exception
{
    /// <summary>
    /// Gets the identifier of the department that caused the exception.
    /// </summary>
    public long DepartmentId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentAlreadyExistsException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DepartmentAlreadyExistsException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DepartmentAlreadyExistsException"/> class with a specified error message and department identifier.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="id">The identifier of the department that caused the exception.</param>
    public DepartmentAlreadyExistsException(string message, long id) : base(message)
    {
        DepartmentId = id;
    }
}