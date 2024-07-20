using EmployeeManager.Model.BaseModel;

namespace EmployeeManager.Data
{
    public class DbInitializer
    {

        public static void Initialize(EmployeeManagerContext context)
        {

           if (context.Department.Any()) { return; } 

            var departments = new Department[]
            {
                new Department { Name = "Human Resources", Abbreviation = "HR", Active = true  },
                new Department { Name = "Information Technology", Abbreviation = "IT", Active = true  },
                new Department { Name = "Finance", Abbreviation = "FIN", Active = false }
                };

            foreach (Department d in departments)
            {
                context.Department.Add(d);
            }
            context.SaveChanges();
        

            if (context.Employee.Any()) { return; }
            

                Employee supervisor1 = new Employee { Name = "John", Active = true, Department = departments[0], Password = "testpassword", PhoneNumber = "06205007447", Position = "manager", Supervisor = null, Username = "John" };
                Employee supervisor2 = new Employee { Name = "Jane", Active = true, Department = departments[1], Password = "testpassword", PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor1, Username = "Jane" };

                var employees = new Employee[]
                {
                new Employee { Name = "James", Active = true, Department = departments[0], Password = "testpassword", PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "James" },
                new Employee { Name = "Jill", Active = true, Department = departments[1], Password = "testpassword", PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "Jill" },
                new Employee { Name = "Jack", Active = true, Department = departments[0], Password = "testpassword", PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "Jack" },
                new Employee { Name = "Jenny", Active = false, Department = departments[1], Password = "testpassword", PhoneNumber = "06205007447", Position = "manager", Supervisor = supervisor2, Username = "Jenny" },


                };

                foreach (Employee e in employees)
                {
                    context.Employee.Add(e);
                }

                context.SaveChanges();
            
        }


    }
}
