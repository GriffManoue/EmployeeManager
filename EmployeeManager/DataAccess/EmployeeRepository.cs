using EmployeeManager.Data;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace EmployeeManager.DataAccess
{
    public class EmployeeRepository : IRepository<Employee>, IDisposable
    {
        private bool disposedValue;
        private readonly EmployeeManagerContext _context;

        public EmployeeRepository(EmployeeManagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                return await _context.Employee.Where(e => e.Active == true).ToListAsync();
            }
            catch (ArgumentNullException e)
            {
                throw new DataAccessException("The list of employees is null.", e);
            }
            catch (OperationCanceledException e)
            {
                throw new DataAccessException("The operation was canceled while retrieving the employees.", e);
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while retrieving the employees.", e);
            }
        }

        public async Task<Employee> GetByIdAsync(long id)
        {
            Employee employee;
            try {

                 employee = await _context.Employee.FindAsync(id);
                return employee;

            }
            catch (Exception e) {
                throw new DataAccessException("An error occurred while retrieving the employee.", e);
            }

           
        }

        public async Task<Employee> InsertAsync(Employee entity)
        {
            Employee employee;
            try
            {
                employee = (await _context.Employee.AddAsync(entity)).Entity;
                return employee;
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while inserting the employee.", e);
            }
           
           
        }

        public Employee Update(Employee entity)
        {
            try {
                _context.Update(entity);
            }catch (Exception e) {
                throw new DataAccessException("An error occurred while updating the employee.", e);
            }
          
            return entity;
        }

        public void Delete(Employee entity)
        {
            try {
                 _context.Employee.Remove(entity);
            }catch (Exception e) {
                throw new DataAccessException("An error occurred while deleting the employee.", e);
            }  
        }
        

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DataAccessException("A concurrency update error occurred while saving the changes.", e);
            }
            catch (DbUpdateException e)
            {
                throw new DataAccessException("An update error occurred while saving the changes.", e);
            }
            catch (OperationCanceledException e) { 
            
                throw new DataAccessException("The operation was canceled while saving the changes.", e);
            }catch (Exception e)
            {
                throw new DataAccessException("An error occurred while saving the changes.", e);
            }          
        }


        public async Task<IQueryable<Employee>> Query(Expression<Func<Employee, bool>> predicate)
        {
            try
            {
                return await Task.FromResult(_context.Employee.Where(predicate));
            }
            catch (ArgumentNullException e) {
                throw new DataAccessException("The predicate is null.", e);
            }catch (Exception e)
            {
                throw new DataAccessException("An error occurred while querying the employees.", e);
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }        
    }
}
