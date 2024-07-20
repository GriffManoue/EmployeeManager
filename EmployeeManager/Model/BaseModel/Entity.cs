using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EmployeeManager.Model.Interfaces;
using System.Text.Json.Serialization;

namespace EmployeeManager.Model.BaseModel
{
    public class Entity : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }

}
