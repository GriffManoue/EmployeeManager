using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManager.Model.Services;

/// <summary>
/// Provides services for hashing and validating passwords for Employee entities.
/// Utilizes the ASP.NET Core Identity <see cref="IPasswordHasher{TUser}"/> for password operations.
/// </summary>
public class PasswordService : IPasswordService<Employee>
{
    private readonly IPasswordHasher<Employee> _passwordHasher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordService"/> class.
    /// </summary>
    /// <param name="passwordHasher">The password hasher to use for hashing and verifying passwords.</param>
    public PasswordService(IPasswordHasher<Employee> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Hashes a given password for an employee.
    /// </summary>
    /// <param name="user">The employee for whom to hash the password.</param>
    /// <param name="password">The password to hash.</param>
    /// <returns>A hashed version of the password.</returns>
    public string HashPassword(Employee user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    /// <summary>
    /// Validates a provided password against a stored hashed password for an employee.
    /// </summary>
    /// <param name="user">The employee for whom to validate the password.</param>
    /// <param name="hashedPassword">The stored hashed password.</param>
    /// <param name="providedPassword">The password to validate.</param>
    /// <returns>True if the provided password matches the hashed password; otherwise, false.</returns>
    public bool ValidatePassword(Employee user, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}