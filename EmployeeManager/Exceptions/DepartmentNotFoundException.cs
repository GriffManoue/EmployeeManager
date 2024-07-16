namespace EmployeeManager.Exceptions
{
    public class DepartmentNotFoundException : Exception
    {
       public long DepartmentId { get; }

        public DepartmentNotFoundException() : base()
        {
        }

        public DepartmentNotFoundException(string message) : base(message)
        {
        }

        public DepartmentNotFoundException(long id)
        {
  
           DepartmentId = id;
        }

        public DepartmentNotFoundException(string message, long id) : base(message)
        {
            DepartmentId = id;
        }


    }
}
