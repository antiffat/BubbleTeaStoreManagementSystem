using BubbleTeaShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task AddEmployeeAsync(Employee employee);
    Task UpdateEmployeeAsync(Employee employee);
    Task DeleteEmployeeAsync(int id);
    Task<bool> EmployeeExistsAsync(int id);
}