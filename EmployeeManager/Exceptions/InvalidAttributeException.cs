namespace EmployeeManager.Exceptions;

/// <summary>
/// Represents errors that occur due to invalid attributes in entities.
/// </summary>
public class InvalidAttributeException : Exception
{

    /// <summary>
    /// Gets or sets the attribute that caused the current exception.
    /// </summary>
    public string? Attribute { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAttributeException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public InvalidAttributeException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAttributeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public InvalidAttributeException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidAttributeException"/> class with a specified error message and the attribute that caused the exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="attribute">The attribute that caused the exception.</param>
    public InvalidAttributeException(string message, string attribute) : base(message)
    {
        this.Attribute = attribute;
    }

    
}