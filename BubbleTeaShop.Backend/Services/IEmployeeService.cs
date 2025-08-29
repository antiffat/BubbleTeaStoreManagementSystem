using BubbleTeaShop.Backend.DTOs.EmployeeDtos;

namespace BubbleTeaShop.Backend.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeDto> GetEmployeeByIdAsync(int id);
    Task<int> CreateEmployeeAsync(AddEmployeeDto dto);
    Task UpdateEmployeeAsync(UpdateEmployeeDto dto);
    Task DeleteEmployeeAsync(int id);
    Task AssignEmployeeToShiftAsync(int employeeId, int shiftId);
    Task RemoveEmployeeFromShiftAsync(int employeeId, int shiftId);
    Task AssignManagerToShiftAsync(int employeeId, int shiftId);
    Task RemoveManagerFromShiftAsync(int shiftId);
    

}