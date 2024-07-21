namespace EmployeeManager.Model.Interfaces;

/// <summary>
/// Defines a basic interface for entity models in the EmployeeManager project.
/// This interface ensures that all implementing entities have common properties.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    long Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the entity.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity is active.
    /// This property can be used to soft delete or deactivate entities without removing them from the database.
    /// </summary>
    bool Active { get; set; }
}