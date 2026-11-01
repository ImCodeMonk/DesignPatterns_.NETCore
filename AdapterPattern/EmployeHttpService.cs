namespace AdapterPattern
{
    public interface IEmployeeHttpService
    {
        List<EmployeHttpDTO> GetEmployeesFromDatabase();
    }
    public class EmployeHttpService : IEmployeeHttpService
    {
        public List<EmployeHttpDTO> GetEmployeesFromDatabase()
        {
            // Simulating database retrieval
            return [
                new ()
                {
                    Name = "John",
                    LastName = "Doe",
                    Address = new()
                    {
                        Address1 = "123 Main St",
                        Address2 = "Apt 4B",
                        PostalCode = "12345",
                    },
                    Salary = "89000" },
                    new()
                    {
                        Name = "Alex",
                        LastName = "Smith",
                        Address = new()
                        {
                            Address1 = "456 Elm St",
                            Address2 = "Suite 200",
                            PostalCode = "67890",
                        },
                    Salary = "83000"
                    },
                    new()
                    {
                        Name = "Dyroth",
                        LastName = "ML",
                        Address = new()
                        {
                            Address1 = "456 Elm St",
                            Address2 = "Square 200",
                            PostalCode = "67890",
                        },
                    Salary = "80000"
                    },
                    new()
                    {
                        Name = "Julian",
                        LastName = "ML",
                        Address = new()
                        {
                            Address1 = "456 Elm St",
                            Address2 = "Manpa 200",
                            PostalCode = "67890",
                        },
                    Salary = "76000"
                    },
                ];
        }
    }
}
