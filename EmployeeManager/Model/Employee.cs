using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManager.Model
{
    public class Employee : Entity
    {
        public string Position  { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Entity? Supervisor { get; set; }
        public Department? Department { get; set; }


    }
}
