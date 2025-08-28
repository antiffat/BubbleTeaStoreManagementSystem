namespace BubbleTesShop.Backend.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<int> CreateEmployeeAsync(AddEmployeeDto dto);
    Task UpdateEmployeeAsync(UpdateEmployeeDto dto);
    Task DeleteEmployeeAsync(int id);
}