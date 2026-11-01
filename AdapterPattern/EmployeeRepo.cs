namespace AdapterPattern
{
    interface IEmployeeRepo
    {
        List<EmployeDTO> GetEmployees();
    }
    public class EmployeeRepo(IAdapter adapter) : IEmployeeRepo
    {
        public List<EmployeDTO> GetEmployees()
        {
            return adapter.GetEmployees();
        }

        
    }
}
