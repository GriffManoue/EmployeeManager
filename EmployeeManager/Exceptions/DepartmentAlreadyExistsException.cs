namespace EmployeeManager.Exceptions
{
    public class DepartmentAlreadyExistsException : Exception
    {
        public long DepartmentId { get; }

        public DepartmentAlreadyExistsException() : base()
        {
        }

        public DepartmentAlreadyExistsException(string message) : base(message)
        {
        }

        public DepartmentAlreadyExistsException(long id)
        {
  
           DepartmentId = id;
        }

        public DepartmentAlreadyExistsException(string message, long id) : base(message)
        {
            DepartmentId = id;
        }

    }
}
