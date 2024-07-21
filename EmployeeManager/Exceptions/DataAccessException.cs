namespace EmployeeManager.Exceptions;

/// <summary>
/// Represents errors that occur during application execution within the data access layer.
/// </summary>
public class DataAccessException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DataAccessException(string message) : base(message)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DataAccessException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not null, the current exception is raised in a catch block that handles the inner exception.</param>
    public DataAccessException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

