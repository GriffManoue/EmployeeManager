using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManager.Data;
using EmployeeManager.Model.Interfaces;
using AutoMapper;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model;

namespace EmployeeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
       
        private readonly ILogicService<Employee> _logicService;
        private readonly IMapper _mapper;

        public EmployeesController(ILogicService<Employee> logicService, IMapper mapper)
        {
            _logicService = logicService;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployee()
        {
           var query = await _logicService.Query(new QueryRequest("active", "true"));
            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(query);
            return Ok(employeeDTOs);

        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployee(long id)
        {
            try {
            
                var employee = await _logicService.GetByIdAsync(id);
                return _mapper.Map<EmployeeDTO>(employee);
            }
            catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

          
        }

        //GET: api/Employees/Query
        [HttpGet("Query")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> QueryEmployee([FromQuery] QueryRequest request)
        {
            var query = await _logicService.Query(request);

            var employeeDTOs = _mapper.Map<IEnumerable<EmployeeDTO>>(query);
            return Ok(employeeDTOs);


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

            try {
                var entity = await _logicService.UpdateAsync(employee);
            }catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> PostEmployee(Employee entity)
        {

            Employee employee = null;

            try {
                employee = await _logicService.AddAsync(entity);
            }catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
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
