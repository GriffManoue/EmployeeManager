using EmployeeManager.Data;
using EmployeeManager.DataAccess;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManager.Model.LogicServices
{
    public class EmployeeLogicService : ILogicService<Employee>
    {

        private readonly ILogger<EmployeeLogicService> _logger;
        private readonly IRepository<Employee> _repository;
        private readonly IPasswordService<Employee> _passwordService;

        public EmployeeLogicService(ILogger<EmployeeLogicService> logger, IRepository<Employee> repository, IPasswordService<Employee> passwordService)
        {
            _logger = logger;
            _repository = repository;
            _passwordService = passwordService;
        }

        public async Task<Employee> AddAsync(Employee entity)
        {
            try
            {
                entity.Password = _passwordService.HashPassword(entity,entity.Password);
                await _repository.InsertAsync(entity);
            }
            catch (EmployeeAlreadyExistsException e) { 
            
                _logger.LogError(e.Message,entity.Id);
            }

            return entity;
        }

        public async Task DeleteAsync(Employee entity)
        {
            try {
            
                await _repository.DeleteAsync(entity);
            }
            catch (EmployeeNotFoundException e) {
            
                _logger.LogError(e.Message, entity.Id);
            }
        }

        public async Task<Employee> GetByIdAsync(long id)
        {
            Employee? employee = null;

            try { 
            
               employee = await _repository.GetByIdAsync(id);
            }catch (EmployeeNotFoundException e) {
            
                _logger.LogError( e.Message, id);
            }

            return employee;

        }

        public async Task<IQueryable<Employee>> Query(Expression<Func<Employee, bool>> predicate)
        {

            IQueryable<Employee> employees = null;

           try {
            
            employees  = await _repository.Query(predicate);
           }
           catch (Exception e) {
            
               _logger.LogError("An error occurred while querying the employees.", e);
           }

            return employees;
        }


        public async Task<Employee> UpdateAsync(Employee entity)
        {
            try {
            
             await _repository.UpdateAsync(entity);
            }
            catch (EmployeeNotFoundException e) {
            
                _logger.LogError(e.Message, entity.Id);
            }
            catch (Exception e) {
            
                _logger.LogError("An error occurred while updating the employee.", e);
            }

            return entity;
        }
    }

   
}
