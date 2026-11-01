using AdapterPattern;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IEmployeeRepo, EmployeeRepo>()
    .AddSingleton<IEmployeeHttpService, EmployeHttpService>()
    .AddSingleton<IAdapter, Adapter>()
    .BuildServiceProvider();


var employeeRepo = serviceProvider.GetService<IEmployeeRepo>();
var employees = employeeRepo.GetEmployees();

foreach (var employee in employees)
{
    Console.WriteLine($"Name: {employee.Name}, Address: {employee.Address}, Salary: {employee.Salary}");
}