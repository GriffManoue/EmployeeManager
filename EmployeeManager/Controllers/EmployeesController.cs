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
    public class EmployeesController : ControllerBase
    {
       
        private readonly ILogicService<Employee> _logicService;

        public EmployeesController(ILogicService<Employee> logicService)
        {
            _logicService = logicService;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
           var query = await _logicService.Query(e => true);
           return query.ToList();
            
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(long id)
        {

            var employee = await _logicService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

           return employee;
        }
        //GET: api/Employees/Query
        [HttpGet("Query")]
        public async Task<ActionResult<IEnumerable<Employee>>> QueryEmployee([FromQuery] string attribute, [FromQuery] string value)
        {
            IEnumerable<Employee> query;

            switch (attribute.ToLower())
            {
                case "id":                     
                    query = await _logicService.Query(e => e.Id.Equals(value));
                    break;
                case "name":
                    query = await _logicService.Query(e => e.Name.Contains(value));
                    break;
                case "position":
                    query = await _logicService.Query(e => e.Position.Contains(value));
                    break;
                case "department":
                    query = await _logicService.Query(e => e.Department.Id.Equals(value));
                    break;
                case"supervisorID":
                        query = await _logicService.Query(e => e.Supervisor.Id.Equals(value));
                    break;
                case "username":
                        query = await _logicService.Query(e => e.Username.Contains(value));
                    break;
                case "active":
                        query = await _logicService.Query(e => e.Active.ToString().Contains(value));
                    break;
                default:
                    return BadRequest("Invalid attribute specified.");
            }

            return query.ToList();
        }


        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(long id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            var entity = await _logicService.UpdateAsync(employee);

            if (entity == null) { 
            
                return NotFound();
            }           

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee entity)
        {
            var employee = await _logicService.AddAsync(entity);

            if (employee == null)
            {

                return BadRequest();
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
          
            var employee = await _logicService.GetByIdAsync(id);
            if (employee == null) 
            {
                return NotFound();
            }

            await _logicService.DeleteAsync(employee);
            return NoContent();
        }
    }
}
