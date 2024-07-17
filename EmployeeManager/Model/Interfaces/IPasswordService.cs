using Microsoft.AspNetCore.Identity;

namespace EmployeeManager.Model.Interfaces
{
    public interface IPasswordService<T>
    {
        string HashPassword(T user, string password);

        bool ValidatePassword(T user, string hashedPassword, string providedPassword);
    }
}
