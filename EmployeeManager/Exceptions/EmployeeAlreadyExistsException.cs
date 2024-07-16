namespace EmployeeManager.Exceptions
{
    internal class EmployeeAlreadyExistsException : Exception
    {
        public long EmployeeId { get; }

        public EmployeeAlreadyExistsException() : base()
        {
        }

        public EmployeeAlreadyExistsException(string message) : base(message)
        {
        }

        public EmployeeAlreadyExistsException(long id)
        {
  
           EmployeeId = id;
        }

        public EmployeeAlreadyExistsException(string message, long id) : base(message)
        {
            EmployeeId = id;
        }

    }
}