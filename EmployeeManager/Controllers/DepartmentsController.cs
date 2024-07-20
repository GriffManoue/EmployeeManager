using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Data;
using EmployeeManager.Model.Interfaces;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model;

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
            var query = await _logicService.Query(new QueryRequest("active", "true"));
            return query.ToList();
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(long id)
        {
            try {

                var department = await _logicService.GetByIdAsync(id);
                return department;
            }
            catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);

            }
        }

        //GET: api/Departments/Query
        [HttpGet("Query")]
        public async Task<ActionResult<IEnumerable<Department>>> QueryDepartment([FromQuery] QueryRequest request)
        {
            var query = await _logicService.Query(request);
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

            try {
            
                var entity = await _logicService.UpdateAsync(department);
            }catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(Department entity)
        {
            Department department = null;

            try { 
            
                department= await _logicService.AddAsync(entity);

            }catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(long id)
        {

            try {
            
                await _logicService.DeleteAsync(id);
            }catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }


            return NoContent();
        }
    }
}
