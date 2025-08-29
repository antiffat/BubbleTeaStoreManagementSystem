using BubbleTeaShop.Backend.DTOs.ShiftDtos;
using BubbleTesShop.Backend.DTOs.ShiftDtos;

namespace BubbleTeaShop.Backend.Services;

public interface IShiftService
{
    Task<IEnumerable<ShiftDto>> GetAllShiftsAsync();
    Task<ShiftDto> GetShiftByIdAsync(int id);
    Task<int> AddShiftAsync(AddShiftDto dto);
    Task UpdateShiftAsync(UpdateShiftDto dto);
    Task DeleteShiftAsync(int id);
    Task AddEmployeeToShiftAsync(int shiftId, int employeeId);
    Task RemoveEmployeeFromShiftAsync(int shiftId, int employeeId);
    Task AssignManagerToShiftAsync(int shiftId, int employeeId);
    Task RemoveManagerFromShiftAsync(int shiftId);
    
}