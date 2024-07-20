using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManager.Model.Services
{
    public class PasswordService : IPasswordService<Employee>
    {

        private readonly IPasswordHasher<Employee> _passwordHasher;

        public PasswordService(IPasswordHasher<Employee> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }


        public string HashPassword(Employee user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool ValidatePassword(Employee user, string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }

    }
}
