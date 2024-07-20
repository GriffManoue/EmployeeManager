using EmployeeManager.Data;
using EmployeeManager.DataAccess.Interfaces;
using EmployeeManager.Exceptions;
using EmployeeManager.Model.BaseModel;
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
            try
            {
                return await _context.Department.Where(e => e.Active == true).ToListAsync();
            }
            catch (ArgumentNullException e)
            {
                throw new DataAccessException("The list of departments is null.", e);
            }
            catch (OperationCanceledException e)
            {
                throw new DataAccessException("The operation was canceled while retrieving the departments.", e);
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while retrieving the departments.", e);
            }
           
        }

        public async Task<Department> GetByIdAsync(long id)
        {
           
            Department department;
            try
            {
                department = await _context.Department.FindAsync(id);
                return department;
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while retrieving the department.", e);
            }
          

        }

        public async Task<Department> InsertAsync(Department entity)
        {
            Department department;
            
            try
            {
               department = (await _context.Department.AddAsync(entity)).Entity;
            
                return department;
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while inserting the department.", e);
            }

           
        }

        public Department Update(Department entity)
        {
            try {
                _context.Update(entity);
            }
            catch (Exception e) {
                throw new DataAccessException("An error occurred while updating the department.", e);
            }

            return entity;
        }

        public void Delete(Department entity)
        {
           try {
                 _context.Department.Remove(entity);
            }
            catch (Exception e) {
                throw new DataAccessException("An error occurred while deleting the department.", e);
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
                throw new DataAccessException("A concurrency error occurred while saving the department.", e);
            }
            catch (DbUpdateException e)
            {
                throw new DataAccessException("An error occurred while saving the department.", e);
            }
            catch (OperationCanceledException e) { 
                throw new DataAccessException("The operation was canceled while saving the department.", e);
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while saving the department.", e);
            }
        }


        public async Task<IQueryable<Department>> Query(Expression<Func<Department, bool>> predicate)
        {
            try
            {
                return await Task.FromResult(_context.Department.Where(predicate));
            }
            catch (ArgumentNullException e) {
            throw new DataAccessException("The predicate is null.", e);
            }
            catch (Exception e)
            {
                throw new DataAccessException("An error occurred while querying the departments.", e);
            }
            
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

        
    }
}
