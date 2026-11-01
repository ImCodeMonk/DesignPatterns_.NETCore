namespace CommandPattern
{
    public class EmployeeRepository
    {
        // Initialize the employee list.
        private readonly List<EmployeeModel> _employees = new();

        public void AddEmployee(EmployeeModel employee)
        {
            if (employee == null)
            {
                Console.WriteLine("Employee cannot be null.");
                return;
            }

            _employees.Add(employee);

            Console.WriteLine($"Employee {employee.Name} added successfully.");
        }

        public void UpdateEmployeeById(EmployeeModel updatedEmployee)
        {
            if (updatedEmployee == null)
            {
                Console.WriteLine("Updated employee cannot be null.");
                return;
            }

            var employee = _employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);

            if (employee != null)
            {
                Console.WriteLine($"Updating Employee ID: {employee.Id}");

                // Display old and new values
                if (employee.Name != updatedEmployee.Name)
                {
                    Console.WriteLine($"Name: {employee.Name} -> {updatedEmployee.Name}");
                    employee.Name = updatedEmployee.Name;
                }

                if (employee.Address != updatedEmployee.Address)
                {
                    Console.WriteLine($"Address: {employee.Address} -> {updatedEmployee.Address}");
                    employee.Address = updatedEmployee.Address;
                }

                Console.WriteLine("Employee updated successfully!");

                Console.WriteLine("\nUpdated Employee Details:");
                Console.WriteLine($"ID: {employee.Id}, Name: {employee.Name}, Address: {employee.Address}");
            }
            else
            {
                Console.WriteLine($"Employee with ID {updatedEmployee.Id} not found.");
            }
        }

        public void DeleteEmployeeById(int employeeId)
        {
            var employee = _employees.FirstOrDefault(
                e => e.Id == employeeId);

            if (employee != null)
            {
                Console.WriteLine($"Deleting Employee: {employee.Name}");

                _employees.Remove(employee);

                Console.WriteLine($"Employee {employee.Name} removed successfully!");

                // Display remaining employees
                Console.WriteLine("\nRemaining Employees:");
                GetAllEmployees();
            }
            else
            {
                Console.WriteLine($"Employee with ID {employeeId} not found.");
            }
        }


        public EmployeeModel? GetEmployeeById(int employeeId)
        {
            var employee = _employees.FirstOrDefault(e => e.Id == employeeId);

            if (employee == null)
            {
                Console.WriteLine($"Employee with ID {employeeId} not found.");
                return null;
            }

            Console.WriteLine($"Employee found: {employee.Name}");

            return employee;
        }

        public List<EmployeeModel> GetAllEmployees()
        {
            if (!_employees.Any())
            {
                Console.WriteLine("No employees found.");
                return new List<EmployeeModel>();
            }

            Console.WriteLine("All employees:");

            foreach (var employee in _employees)
            {
                Console.WriteLine(
                    $"ID: {employee.Id}, Name: {employee.Name}, Address: {employee.Address}");
            }

            return _employees;
        }
    }
}
