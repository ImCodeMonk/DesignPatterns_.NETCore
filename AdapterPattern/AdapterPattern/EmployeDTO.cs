namespace AdapterPattern
{
    public class EmployeDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Salary { get; set; }
    }

    public class EmployeAddress
    {
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? PostalCode { get; set; }
    }

    public class EmployeHttpDTO
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public EmployeAddress? Address { get; set; }
        public string? Salary { get; set; }
    }
}
