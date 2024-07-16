using EmployeeManager.Data;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManager.DataAccess
{
    public class DepartmentRepository :IRepository<Department>, IDisposable
    {
        private bool disposedValue;
        private readonly EmployeeManagerContext _context;

        public DepartmentRepository(EmployeeManagerContext context )
        {
            _context = context;
        }


        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Department.ToListAsync();
        }

        public async Task<Department> GetByIdAsync(long id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                throw new DepartmentNotFoundException("A department with the given id was not found.", id);
            }

            return department;
        }

        public async Task<Department> InsertAsync(Department entity)
        {
            

            if (DepartmentExists(entity.Id))
            {

                throw new DepartmentAlreadyExistsException("A department with the given id already exists.", entity.Id);
            }
            var department = _context.Department.Add(entity);
            await _context.SaveChangesAsync();
            return department.Entity;
        }

        public async Task UpdateAsync(Department entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(entity.Id))
                {
                    throw new DepartmentNotFoundException("A department with the given id was not found.", entity.Id);
                }
                else
                {
                    throw new Exception("An error occurred while updating the department.");
                }
            }
        }

        public async Task DeleteAsync(Department entity)
        {
            var department = await _context.Department.FindAsync(entity.Id);
            if (department == null)
            {
                throw new DepartmentNotFoundException("A department with the given id was not found.", entity.Id);
            }

            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<IQueryable<Department>> Query(Expression<Func<Department, bool>> predicate)
        {
            return await Task.FromResult(_context.Department.Where(predicate));
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

        public void Dispose()
        {
            
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool DepartmentExists(long id)
        {
            return _context.Department.Any(e => e.Id == id);
        }
    }
}
