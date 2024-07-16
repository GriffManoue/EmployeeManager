namespace EmployeeManager.Exceptions
{
    public class EmployeeNotFoundException : Exception
    {

        public long EmployeeId { get; }

        public EmployeeNotFoundException() : base() { }

        public EmployeeNotFoundException(string message) : base(message) { }

        public EmployeeNotFoundException(long id) {
            EmployeeId = id;
        }

        public EmployeeNotFoundException(string message, long id) : base(message)
        {
            EmployeeId = id;
        }

        
    }
}
