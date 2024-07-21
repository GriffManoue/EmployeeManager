namespace EmployeeManager.Model.Interfaces;

/// <summary>
/// Defines a service for handling password operations such as hashing and validation.
/// </summary>
/// <typeparam name="T">The type of the user object that the password is associated with.</typeparam>
public interface IPasswordService<T>
{
    /// <summary>
    /// Hashes a password for a given user.
    /// </summary>
    /// <param name="user">The user for whom the password is being hashed.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed version of the provided password.</returns>
    string HashPassword(T user, string password);

    /// <summary>
    /// Validates a provided password against a stored hashed password for a given user.
    /// </summary>
    /// <param name="user">The user for whom the password validation is being performed.</param>
    /// <param name="hashedPassword">The stored hashed password to validate against.</param>
    /// <param name="providedPassword">The provided password to validate.</param>
    /// <returns>True if the provided password matches the hashed password; otherwise, false.</returns>
    bool ValidatePassword(T user, string hashedPassword, string providedPassword);
}