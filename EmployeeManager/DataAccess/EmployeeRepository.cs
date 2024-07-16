using EmployeeManager.Data;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Employee.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(long id)
        {
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                throw new EmployeeNotFoundException("An employee with the given id was not found.",id);
            }

            return employee;
        }

        public async Task<Employee> InsertAsync(Employee entity)
        {
            if (EmployeeExists(entity.Id)) {
            
                throw new EmployeeAlreadyExistsException("An employee with the given id already exists.", entity.Id);
            }
            var employee = _context.Employee.Add(entity);
            await _context.SaveChangesAsync();
            return employee.Entity;
        }

        public async Task UpdateAsync(Employee entity)
        {
           

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(entity.Id))
                {
                    throw new EmployeeNotFoundException("An employee with the given id was not found.", entity.Id);
                }
                else
                {
                    throw new Exception("An error occurred while updating the employee.");
                }
            }

            
        }

        public async Task DeleteAsync(Employee entity)
        {
            var employee = await _context.Employee.FindAsync(entity.Id);
            if (employee == null)
            {
                throw new EmployeeNotFoundException("An employee with the given id was not found.", entity.Id);
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
        
        }
        

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<IQueryable<Employee>> Query(Expression<Func<Employee, bool>> predicate)
        {
            return await Task.FromResult(_context.Employee.Where(predicate));
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

        private bool EmployeeExists(long id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

        
    }
}
