namespace AdapterPattern
{
    public interface IAdapter
    {
        List<EmployeDTO> GetEmployees();
    }
    internal class Adapter(IEmployeeHttpService employeeHttpService) : IAdapter
    {
        public List<EmployeDTO> GetEmployees()
        {
            return employeeHttpService.GetEmployeesFromDatabase().Select(e => new EmployeDTO
            {
                Name = $"{e.Name} {e.LastName}",
                Address = $"{e.Address?.Address1}, {e.Address?.Address2}, {e.Address?.PostalCode}",
                Salary = e.Salary
            }).ToList();
        }
    }
}
