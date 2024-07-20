namespace EmployeeManager.Exceptions
{
    public class InvalidAttributeException : Exception
    {

        public string attribute { get; set; }
        public InvalidAttributeException(string message) : base(message)
        {
        }

        public InvalidAttributeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidAttributeException(string message, string attribute) : base(message)
        {
            this.attribute = attribute;
        }


    }
}
