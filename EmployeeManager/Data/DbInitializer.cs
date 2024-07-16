using EmployeeManager.Model;

namespace EmployeeManager.Data
{
    public class DbInitializer
    {

        public static void Initialize(EmployeeManagerContext context) {

            if (context.Employee.Any()) { return; }

            var employees = new Employee[]
            {
                new Employee { Name = "John Doe", Position = "Manager", PhoneNumber = "555-555-5555", Username = "johndoe", Password = "password" },
                new Employee { Name = "Jane Doe", Position = "Assistant Manager", PhoneNumber = "555-555-5555", Username = "janedoe", Password = "password" },
                new Employee { Name = "Jim Doe", Position = "Employee", PhoneNumber = "555-555-5555", Username = "jimdoe", Password = "password" }
            };

            foreach (Employee e in employees) {
                context.Employee.Add(e);
            }

            context.SaveChanges();

            var departments = new Department[]
            {
                new Department { Name = "Human Resources", Abbreviation = "HR" },
                new Department { Name = "Information Technology", Abbreviation = "IT" },
                new Department { Name = "Accounting", Abbreviation = "ACCT" }
            };
            foreach (Department e in departments) {
                context.Department.Add(e);
            }
            context.SaveChanges();


        
        
        
        }


    }
}
