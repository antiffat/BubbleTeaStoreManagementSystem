using BubbleTeaShop.Backend.DTOs.ShiftDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;
using BubbleTeaShop.Backend.Repositories;
using BubbleTesShop.Backend.DTOs.ShiftDtos;
using BubbleTesShop.Backend.Models;

namespace BubbleTeaShop.Backend.Services;

public class ShiftService : IShiftService
{
    private readonly IShiftRepository _shiftRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ShiftService(IShiftRepository shiftRepository, IEmployeeRepository employeeRepository)
    {
        _shiftRepository = shiftRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<ShiftDto>> GetAllShiftsAsync()
    {
        var shifts = await _shiftRepository.GetAllShiftsAsync();
        return shifts.Select(MapToDto);
    }

    public async Task<ShiftDto> GetShiftByIdAsync(int id)
    {
        if (!await _shiftRepository.ShiftExistsAsync(id))
            throw new KeyNotFoundException("Shift with given ID does not exist.");

        var shift = await _shiftRepository.GetShiftByIdAsync(id);
        return MapToDto(shift);
    }

    public async Task<int> AddShiftAsync(AddShiftDto dto)
    {
        if (dto.StartTime >= dto.EndTime)
            throw new ArgumentException("StartTime must be before EndTime.");

        if (!await _employeeRepository.EmployeeExistsAsync(dto.ManagingEmployeeId))
            throw new KeyNotFoundException("Managing employee does not exist.");

        var manager = await _employeeRepository.GetEmployeeByIdAsync(dto.ManagingEmployeeId);
        if (manager == null)
            throw new KeyNotFoundException("Managing employee does not exist.");

        if (!(manager.EmployeeRoles?.Any(r => r.Role == EmployeeRole.MANAGER) ?? false))
            throw new InvalidOperationException("Managing employee must have MANAGER role.");

        var employeeIds = dto.EmployeeIds?.Distinct().ToList() ?? new List<int>();
        if (!employeeIds.Contains(dto.ManagingEmployeeId))
            employeeIds.Add(dto.ManagingEmployeeId);

        var employees = new List<Employee>();
        foreach (var empId in employeeIds)
        {
            if (!await _employeeRepository.EmployeeExistsAsync(empId))
                throw new KeyNotFoundException($"Employee with id {empId} does not exist.");

            var e = await _employeeRepository.GetEmployeeByIdAsync(empId);
            if (e == null)
                throw new KeyNotFoundException($"Employee with id {empId} does not exist.");
            employees.Add(e);
        }

        var shift = new Shift
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            DayOfWeek = dto.DayOfWeek,
            ManagingEmployeeId = dto.ManagingEmployeeId,
            Employees = employees
        };

        await _shiftRepository.AddShiftAsync(shift);
        return shift.Id;
    }

    public async Task UpdateShiftAsync(UpdateShiftDto dto)
    {
        if (!await _shiftRepository.ShiftExistsAsync(dto.Id))
            throw new KeyNotFoundException("Shift with given ID does not exist.");

        var existing = await _shiftRepository.GetShiftByIdAsync(dto.Id);
        if (existing == null)
            throw new KeyNotFoundException("Shift with given ID does not exist.");

        if (dto.StartTime >= dto.EndTime)
            throw new ArgumentException("StartTime must be before EndTime.");

        existing.StartTime = dto.StartTime;
        existing.EndTime = dto.EndTime;
        existing.DayOfWeek = dto.DayOfWeek;

        if (dto.ManagingEmployeeId.HasValue)
        {
            var newManagerId = dto.ManagingEmployeeId.Value;
            if (!await _employeeRepository.EmployeeExistsAsync(newManagerId))
                throw new KeyNotFoundException("Managing employee does not exist.");

            var manager = await _employeeRepository.GetEmployeeByIdAsync(newManagerId);
            if (!(manager.EmployeeRoles?.Any(r => r.Role == EmployeeRole.MANAGER) ?? false))
                throw new InvalidOperationException("Managing employee must have MANAGER role.");

            existing.ManagingEmployeeId = newManagerId;

            if (existing.Employees == null) existing.Employees = new List<Employee>();
            if (!existing.Employees.Any(e => e.Id == newManagerId))
                existing.Employees.Add(manager);
        }

        if (dto.EmployeeIds != null)
        {
            var newIds = dto.EmployeeIds.Distinct().ToList();

            if (existing.ManagingEmployeeId.HasValue && !newIds.Contains(existing.ManagingEmployeeId.Value))
                newIds.Add(existing.ManagingEmployeeId.Value);

            var newEmployees = new List<Employee>();
            foreach (var empId in newIds)
            {
                if (!await _employeeRepository.EmployeeExistsAsync(empId))
                    throw new KeyNotFoundException($"Employee with id {empId} does not exist.");
                var e = await _employeeRepository.GetEmployeeByIdAsync(empId);
                newEmployees.Add(e);
            }

            existing.Employees = newEmployees;
        }

        await _shiftRepository.UpdateShiftAsync(existing);
    }

    public async Task DeleteShiftAsync(int id)
    {
        if (!await _shiftRepository.ShiftExistsAsync(id))
            throw new KeyNotFoundException("Shift with given ID does not exist.");

        await _shiftRepository.DeleteShiftAsync(id);
    }

    public async Task AddEmployeeToShiftAsync(int shiftId, int employeeId)
    {
        if (!await _shiftRepository.ShiftExistsAsync(shiftId))
            throw new KeyNotFoundException("Shift does not exist.");
        if (!await _employeeRepository.EmployeeExistsAsync(employeeId))
            throw new KeyNotFoundException("Employee does not exist.");

        var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
        if (shift == null) throw new KeyNotFoundException("Shift does not exist.");

        if (shift.Employees == null) shift.Employees = new List<Employee>();
        if (shift.Employees.Any(e => e.Id == employeeId)) return; // idempotent

        var emp = await _employee_repository_get(employeeId);
        shift.Employees.Add(emp);

        await _shiftRepository.UpdateShiftAsync(shift);
    }

    public async Task RemoveEmployeeFromShiftAsync(int shiftId, int employeeId)
    {
        if (!await _shiftRepository.ShiftExistsAsync(shiftId))
            throw new KeyNotFoundException("Shift does not exist.");

        var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
        if (shift == null) throw new KeyNotFoundException("Shift does not exist.");

        if (shift.ManagingEmployeeId.HasValue && shift.ManagingEmployeeId.Value == employeeId)
            throw new InvalidOperationException("Cannot remove the employee who is managing this shift. Reassign or remove manager first.");

        await _shiftRepository.RemoveEmployeeFromShiftAsync(shiftId, employeeId);
    }

    public async Task AssignManagerToShiftAsync(int shiftId, int employeeId)
    {
        if (!await _shiftRepository.ShiftExistsAsync(shiftId))
            throw new KeyNotFoundException("Shift does not exist.");
        if (!await _employeeRepository.EmployeeExistsAsync(employeeId))
            throw new KeyNotFoundException("Employee does not exist.");

        var emp = await _employee_repository_get(employeeId);
        if (!(emp.EmployeeRoles?.Any(r => r.Role == EmployeeRole.MANAGER) ?? false))
            throw new InvalidOperationException("Employee must have MANAGER role to be assigned manager.");

        await _shiftRepository.AssignManagerToShiftAsync(shiftId, employeeId);
    }

    public async Task RemoveManagerFromShiftAsync(int shiftId)
    {
        if (!await _shiftRepository.ShiftExistsAsync(shiftId))
            throw new KeyNotFoundException("Shift does not exist.");

        await _shiftRepository.RemoveManagerFromShiftAsync(shiftId);
    }

    private ShiftDto MapToDto(Shift s)
    {
        if (s == null) return null;
        return new ShiftDto
        {
            Id = s.Id,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            DayOfWeek = s.DayOfWeek,
            EmployeeIds = s.Employees?.Select(e => e.Id).ToList() ?? new List<int>(),
            ManagingEmployeeId = s.ManagingEmployeeId,
            ManagingEmployeeName = s.ManagingEmployee?.FullName
        };
    }

    private async Task<Employee> _employee_repository_get(int id) =>
        await _employeeRepository.GetEmployeeByIdAsync(id);

}