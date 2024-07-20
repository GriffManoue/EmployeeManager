namespace EmployeeManager.Model
{
    public class QueryRequest
    {
        public string Attribute { get; set; }
        public string Value { get; set; }

        public QueryRequest(string attribute, string value)
        {
            Attribute = attribute;
            Value = value;
        }

        public QueryRequest()
        {
        }
    }
}
