using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Model;

namespace EmployeeManager.Data
{
    public class EmployeeManagerContext : DbContext
    {
        public EmployeeManagerContext (DbContextOptions<EmployeeManagerContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Department> Department { get; set; }
    }
}
