using EmployeeManager.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EmployeeManager.Model
{
    public class Department : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Abbreviation { get; set; }
    }
}
