namespace EmployeeManager.Model;

/// <summary>
/// Represents a request for querying entities based on specific attributes and values.
/// </summary>
public class QueryRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryRequest"/> class with specified attribute and value.
    /// </summary>
    /// <param name="attribute">The attribute to query by.</param>
    /// <param name="value">The value of the attribute to query for.</param>
    public QueryRequest(string attribute, string value)
    {
        Attribute = attribute;
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryRequest"/> class.
    /// </summary>
    public QueryRequest()
    {
    }

    /// <summary>
    /// Gets or sets the attribute to query by.
    /// </summary>
    public string Attribute { get; set; }

    /// <summary>
    /// Gets or sets the value of the attribute to query for.
    /// </summary>
    public string Value { get; set; }
}