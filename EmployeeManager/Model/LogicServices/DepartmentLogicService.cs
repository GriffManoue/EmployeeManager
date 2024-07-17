using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.Interfaces;
using System.Linq.Expressions;

namespace EmployeeManager.Model.LogicServices
{
    public class DepartmentLogicService : ILogicService<Department>
    {
        private readonly ILogger<DepartmentLogicService> _logger;
        private readonly IRepository<Department> _repository;

        public DepartmentLogicService(ILogger<DepartmentLogicService> logger, IRepository<Department> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Department> AddAsync(Department entity)
        {
            Department? department = null;

            try
            {
               department = await _repository.InsertAsync(entity);
            }
            catch (DepartmentAlreadyExistsException e)
            {
                _logger.LogError(e.Message, entity.Id);
            }

            return entity;
        }

        public async Task DeleteAsync(Department entity)
        {
            try
            {

                await _repository.DeleteAsync(entity);
            }
            catch (DepartmentNotFoundException e)
            {

                _logger.LogError(e.Message, entity.Id);
            }
        }

        public async Task<Department> GetByIdAsync(long id)
        {
            Department? department = null;

            try
            {

                department = await _repository.GetByIdAsync(id);
            }
            catch (DepartmentNotFoundException e)
            {

                _logger.LogError(e.Message, id);
            }

            return department;
        }

        public async Task<IQueryable<Department>> Query(Expression<Func<Department, bool>> predicate)
        {
            IQueryable<Department> employees = null;

            try
            {

                employees = await _repository.Query(predicate);
            }
            catch (Exception e)
            {

                _logger.LogError("An error occurred while querying the departments.", e);
            }

            return employees;
        }

        public async Task<Department> UpdateAsync(Department entity)
        {
            try
            {

                await _repository.UpdateAsync(entity);
            }
            catch (DepartmentNotFoundException e)
            {

                _logger.LogError(e.Message, entity.Id);
            }
            catch (Exception e)
            {

                _logger.LogError("An error occurred while updating the department.", e);
            }

            return entity;
        }
    }
}
