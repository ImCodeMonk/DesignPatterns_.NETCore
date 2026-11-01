namespace CommandPattern;

interface ICommand
{
    void Execute();
}

class AddEmployeeCommand(EmployeeModel employee, EmployeeRepository repository) : ICommand
{
    public void Execute()
    {
        repository.AddEmployee(employee);
    }
}

class UpdateEmployeeCommand(EmployeeModel updatedEmployee, EmployeeRepository repository) : ICommand
{
    public void Execute()
    {
        repository.UpdateEmployeeById(updatedEmployee);
    }
}

public class DeleteEmployeeCommand(int employeeId, EmployeeRepository repository) : ICommand
{
    public void Execute()
    {
        repository.DeleteEmployeeById(employeeId);
    }
}

public class GetEmployeesCommand(EmployeeRepository repository) : ICommand
{
    public void Execute()
    {
        var employees = repository.GetAllEmployees();
        if (employees != null && employees.Any())
        {
            Console.WriteLine("Employee List:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"ID: {employee.Id}, Name: {employee.Name}, Address: {employee.Address}");
            }
        }
        else
        {
            Console.WriteLine("No employees found.");
        }
    }
}


