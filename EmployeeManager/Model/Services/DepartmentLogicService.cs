using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using EmployeeManager.Model.Interfaces;
using System.Linq.Expressions;

namespace EmployeeManager.Model.LogicServices
{
    public class DepartmentLogicService : ILogicService<Department>
    {
        private readonly ILogger<DepartmentLogicService> _logger;
        private readonly IRepository<Department> _repository;
        private Dictionary<string, Func<string, Task<IEnumerable<Department>>>> _queryActions = new();

        public DepartmentLogicService(ILogger<DepartmentLogicService> logger, IRepository<Department> repository)
        {
            _logger = logger;
            _repository = repository;

            _queryActions.Add("id", async x => new List<Department> { await _repository.GetByIdAsync(long.Parse(x)) });
            _queryActions.Add("name", async x => await _repository.Query(d => d.Name.Contains(x)));
            _queryActions.Add("abbreviation", async x => await _repository.Query(d => d.Abbreviation.Contains(x)));
            _queryActions.Add("active", async x => await _repository.Query(d => d.Active.ToString().Contains(x)));
        }

        public async Task<Department> AddAsync(Department entity)
        {
           Department department = null;

            try
            {
                department = await _repository.GetByIdAsync(entity.Id);
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }

            if (department == null)
            {
                try
                {
                    await _repository.InsertAsync(entity);
                    await _repository.SaveAsync();
                    return entity;
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }else
            {
                _logger.LogError("The department with the given id already exists.", entity.Id);
                throw new DepartmentAlreadyExistsException("The department with the given id already exists.", entity.Id);
            }

            return null;

        }

        public async Task DeleteAsync(long id)
        {
            try {
            
                var department = await _repository.GetByIdAsync(id);

                if (department != null) {
                
                    _repository.Delete(department);
                    await _repository.SaveAsync();
                }else {
                    _logger.LogError("The department with the given id was not found.", id);
                    throw new DepartmentNotFoundException("The department with the given id was not found.", id);
                }
            
            
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        public async Task<Department> GetByIdAsync(long id)
        {
            Department department = null;

            try
            {

                department = await _repository.GetByIdAsync(id);


                if (department == null)
                {
                    _logger.LogError("The department with the given id was not found.", id);
                    throw new DepartmentNotFoundException("The department with the given id was not found.", id);
                }
            }
            catch (DepartmentNotFoundException e)
            {

                _logger.LogError(e.Message, id);
            }

            return department;
        }

        public async Task<IQueryable<Department>> Query(QueryRequest request)
        {
            var attribute = request.Attribute.ToLower();
            var value = request.Value.ToLower();

            if (!_queryActions.ContainsKey(attribute))
            {
                _logger.LogError("The attribute is not valid.", attribute);
                throw new InvalidAttributeException("The attribute is not valid.", attribute);
            }

            var result = await _queryActions[attribute](value);

            return result.AsQueryable();
        }

        public async Task<Department> UpdateAsync(Department entity)
        {
            Department department = null;

            try
            {
                department = await _repository.GetByIdAsync(entity.Id);

                if (department != null)
                {
                    _repository.Update(entity);
                    await _repository.SaveAsync();
                }
                else
                {
                    _logger.LogError("The department with the given id was not found.", entity.Id);
                    throw new DepartmentNotFoundException("The department with the given id was not found.", entity.Id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return department;
        }
    }
}
