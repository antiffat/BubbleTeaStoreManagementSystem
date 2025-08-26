using BubbleTeaShop.Backend.Helpers;
using BubbleTesShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Repositories;

public class ShiftRepository : IShiftRepository
{
    private readonly ApplicationDbContext _context;

    public ShiftRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
    {
        return await _context.Shifts
            .Include(s => s.Employees)
            .Include(s => s.ManagingEmployee)
            .ToListAsync();
    }
    
    public async Task<Shift> GetShiftByIdAsync(int id)
    {
        return await _context.Shifts
            .Include(s => s.Employees)
            .Include(s => s.ManagingEmployee)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task AddShiftAsync(Shift shift)
    {
        await _context.Shifts.AddAsync(shift);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateShiftAsync(Shift shift)
    {
        _context.Shifts.Update(shift);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteShiftAsync(int id)
    {
        var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == id);
        if (shift != null)
        {
            _context.Shifts.Remove(shift);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> ShiftExistsAsync(int id)
    {
        return await _context.Shifts.AnyAsync(s => s.Id == id);
    }
    
    public async Task AddEmployeeToShiftAsync(int shiftId, int employeeId)
    {
        var shift = await _context.Shifts
            .Include(s => s.Employees)
            .FirstOrDefaultAsync(s => s.Id == shiftId);

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (shift != null && employee != null && !shift.Employees.Any(e => e.Id == employeeId))
        {
            shift.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task RemoveEmployeeFromShiftAsync(int shiftId, int employeeId)
    {
        var shift = await _context.Shifts
            .Include(s => s.Employees)
            .FirstOrDefaultAsync(s => s.Id == shiftId);

        if (shift != null)
        {
            var employeeToRemove = shift.Employees.FirstOrDefault(e => e.Id == employeeId);
            if (employeeToRemove != null)
            {
                shift.Employees.Remove(employeeToRemove);
                await _context.SaveChangesAsync();
            }
        }
    }
    
    public async Task AssignManagerToShiftAsync(int shiftId, int employeeId)
    {
        var shift = await _context.Shifts
            .Include(s => s.Employees)
            .FirstOrDefaultAsync(s => s.Id == shiftId);

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employeeId);

        if (shift != null && employee != null)
        {
            shift.ManagingEmployeeId = employeeId;
            // A manager also works the shift, as per the subset relationship
            if (!shift.Employees.Any(e => e.Id == employeeId))
            {
                shift.Employees.Add(employee);
            }
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task RemoveManagerFromShiftAsync(int shiftId)
    {
        var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.Id == shiftId);
        if (shift != null)
        {
            shift.ManagingEmployeeId = null;
            await _context.SaveChangesAsync();
        }
    }
}