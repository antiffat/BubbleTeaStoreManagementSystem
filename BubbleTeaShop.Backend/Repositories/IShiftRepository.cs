using BubbleTesShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IShiftRepository
{
    Task<IEnumerable<Shift>> GetAllShiftsAsync();
    Task<Shift> GetShiftByIdAsync(int id);
    Task AddShiftAsync(Shift shift);
    Task UpdateShiftAsync(Shift shift);
    Task DeleteShiftAsync(int id);
    Task<bool> ShiftExistsAsync(int id);
    Task AddEmployeeToShiftAsync(int shiftId, int employeeId);
    Task RemoveEmployeeFromShiftAsync(int shiftId, int employeeId);
    Task AssignManagerToShiftAsync(int shiftId, int employeeId);
    Task RemoveManagerFromShiftAsync(int shiftId);
}