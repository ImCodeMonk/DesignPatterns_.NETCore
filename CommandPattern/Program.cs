using CommandPattern;

//var employeeRepository = new EmployeeRepository();
//employeeRepository.AddEmployee(new EmployeeModel { Id = 1, Name = "John Doe", Address = "Kothrud, Pune, MH" });
//employeeRepository.AddEmployee(new EmployeeModel { Id = 2, Name = "Shubham Nehere", Address = "Katraj, Pune, MH" });
//employeeRepository.GetAllEmployees()?.ForEach(e => Console.WriteLine($"Employee ID: {e.Id}, Name: {e.Name}, Address: {e.Address}"));
//employeeRepository.UpdateEmployeeById(new EmployeeModel { Id = 1, Name = "John Smith", Address = "Swargate, Pune, MH" });
//employeeRepository.DeleteEmployeeById(1);

var invoker = new CommandInvoker();
invoker.AddCommand(new AddEmployeeCommand(new EmployeeModel { Id = 1, Name = "John Doe", Address = "Kothrud, Pune, MH" }, new EmployeeRepository()));
invoker.AddCommand(new AddEmployeeCommand(new EmployeeModel { Id = 2, Name = "Shubham Nehere", Address = "Katraj, Pune, MH" }, new EmployeeRepository()));
invoker.AddCommand(new GetEmployeesCommand(new EmployeeRepository()));
invoker.AddCommand(new UpdateEmployeeCommand(new EmployeeModel { Id = 1, Name = "John Smith", Address = "Swargate, Pune, MH" }, new EmployeeRepository()));
invoker.AddCommand(new AddEmployeeCommand(new EmployeeModel { Id = 3, Name = "Alice Johnson", Address = "Baner, Pune, MH" }, new EmployeeRepository()));
invoker.AddCommand(new DeleteEmployeeCommand(1, new EmployeeRepository()));

invoker.ExecuteCommands();