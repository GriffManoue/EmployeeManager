using AutoMapper;
using EmployeeManager.Data;
using EmployeeManager.DataAccess;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;

namespace EmployeeManager.Model.LogicServices
{
    public class EmployeeLogicService : ILogicService<Employee>
    {

        private readonly ILogger<EmployeeLogicService> _logger;
        private readonly IRepository<Employee> _EmployeeRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IPasswordService<Employee> _passwordService;
        private Dictionary<string, Func<string, Task<IEnumerable<Employee>>>> _queryActions = new();


        public EmployeeLogicService(ILogger<EmployeeLogicService> logger, IRepository<Employee> EmployeeRepository, IRepository<Department> DepartmentRepository, IPasswordService<Employee> passwordService)
        {
            _logger = logger;
            _EmployeeRepository = EmployeeRepository;
            _passwordService = passwordService;
            _departmentRepository = DepartmentRepository;

            _queryActions.Add("id", async x => new List<Employee> { await _EmployeeRepository.GetByIdAsync(long.Parse(x)) });
            _queryActions.Add("name", async x => await _EmployeeRepository.Query(e => e.Name.Contains(x)));
            _queryActions.Add("department", async x => await _EmployeeRepository.Query(e => e.Department.Name.Contains(x)));
            _queryActions.Add("active", async x => await _EmployeeRepository.Query(e => e.Active == bool.Parse(x)));
            _queryActions.Add("position", async x => await _EmployeeRepository.Query( e => e.Position.Contains(x)));
            _queryActions.Add("phone", async x => await _EmployeeRepository.Query(e => e.PhoneNumber.Contains(x)));
            _queryActions.Add("username", async x => await _EmployeeRepository.Query(e => e.Username.Contains(x)));
            _queryActions.Add("supervisor", async x => await _EmployeeRepository.Query(e => e.Supervisor.Name.Contains(x)));

        }

        public async Task<Employee> AddAsync(Employee entity)
        {

            Employee employee = null;

            try
            {
                employee = await _EmployeeRepository.GetByIdAsync(entity.Id);
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }


            if (employee == null)
            {
                entity.Password = _passwordService.HashPassword(entity, entity.Password);
                try
                {
                    var department = await _departmentRepository.GetByIdAsync(entity.Department.Id);

                    if (department == null)
                    {
                        _logger.LogError("The department with the given id was not found.", entity.Department.Id);
                        throw new DepartmentNotFoundException("The department with the given id was not found.", entity.Department.Id);
                    }

                    await _EmployeeRepository.InsertAsync(entity);
                    await _EmployeeRepository.SaveAsync();
                    return entity;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }

            }
            else
            {

                _logger.LogError("An employee with the given id already exists.", entity.Id);
                throw new EmployeeAlreadyExistsException("An employee with the given id already exists.", entity.Id);
            }

            return null;
           
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                var employee = await _EmployeeRepository.GetByIdAsync(id);

                if (employee != null)
                {
                    _EmployeeRepository.Delete(employee);
                    await _EmployeeRepository.SaveAsync();
                }
                else
                {

                    _logger.LogError("An employee with the given id was not found.", id);
                    throw new EmployeeNotFoundException("An employee with the given id was not found.", id);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

        }

        public async Task<Employee> GetByIdAsync(long id)
        {
            Employee employee = null;
            try
            {
                employee = await _EmployeeRepository.GetByIdAsync(id);

                if (employee == null)
                {
                    _logger.LogError("An employee with the given id was not found.", id);
                    throw new EmployeeNotFoundException("An employee with the given id was not found.", id);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return employee;
        }

        public async Task<IQueryable<Employee>> Query(QueryRequest request)
        {
            var attribute = request.Attribute.ToLower();
            var value = request.Value.ToLower();

            if (!_queryActions.ContainsKey(attribute))
            {
                _logger.LogError("The attribute is not valid.", attribute);
                throw new InvalidAttributeException("The attribute is not valid.", attribute);
            }

            return  (await _queryActions[attribute](value)).AsQueryable();

            
        }


        public async Task<Employee> UpdateAsync(Employee entity)
        {

            Employee employee = null;

            try
            {
                employee = await _EmployeeRepository.GetByIdAsync(entity.Id);
                var department = await _departmentRepository.GetByIdAsync(entity.Department.Id);

                if (employee != null)
                {

                    if (department == null)
                    {
                       _logger.LogError("The department with the given id was not found.", entity.Department.Id);
                        throw new DepartmentNotFoundException("The department with the given id was not found.", entity.Department.Id);
                    }
                    if (entity.Password != employee.Password)
                    {
                        entity.Password = _passwordService.HashPassword(entity, entity.Password);
                    }

                    _EmployeeRepository.Update(entity);
                    await _EmployeeRepository.SaveAsync();

                }
                else
                {
                    _logger.LogError("An employee with the given id was not found.", entity.Id);
                    throw new EmployeeNotFoundException("An employee with the given id was not found.", entity.Id);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return employee;
        }
    }

   
}
