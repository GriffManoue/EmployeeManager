using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Data;
using EmployeeManager.Model;
using EmployeeManager.Model.Interfaces;

namespace EmployeeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ILogicService<Department> _logicService;

        public DepartmentsController(ILogicService<Department> logicService)
        {
           _logicService = logicService;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartment()
        {
            var query = await _logicService.Query(e => true);
            return query.ToList();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(long id)
        {
           var department = await _logicService.GetByIdAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        //GET: api/Departments/Query
        [HttpGet("Query")]
        public async Task<ActionResult<IEnumerable<Department>>> QueryDepartment([FromQuery] string attribute, [FromQuery] string value)
        {
            IEnumerable<Department> query;

            switch (attribute.ToLower())
            {
                case "name":
                    query = await _logicService.Query(d => d.Name.Contains(value));
                    break;
                case "id":
                    query = await _logicService.Query(d => d.Id.Equals(value));
                    break;
                case "abbreviation":
                    query = await _logicService.Query(d => d.Abbreviation.Contains(value));
                    break;
                // Add more cases for other department-specific attributes you want to filter by
                case "active":
                    query = await _logicService.Query(d => d.Active.ToString().Contains(value));
                    break;
                default:
                    return BadRequest("Invalid attribute specified.");
            }

            return query.ToList();
        }


        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(long id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            var entity = await _logicService.UpdateAsync(department);

            if (entity == null)
            {

                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department entity)
        {
            var department = await _logicService.AddAsync(entity);

            if (department == null) { 
            
                return BadRequest();
            }

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {
            var department = await _logicService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _logicService.DeleteAsync(department);
            return NoContent();
        }
    }
}
