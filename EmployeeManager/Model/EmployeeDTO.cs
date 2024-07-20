namespace EmployeeManager.Model
{
    public class EmployeeDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public long SupervisorId { get; set; }
        public long DepartmentId { get; set; }

        public bool Active { get; set; }




    }
}
