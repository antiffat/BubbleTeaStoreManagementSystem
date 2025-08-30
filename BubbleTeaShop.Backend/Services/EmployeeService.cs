using BubbleTeaShop.Backend.DTOs.AssignmentHistoryDtos;
using BubbleTeaShop.Backend.DTOs.EmployeeDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;
using BubbleTeaShop.Backend.Repositories;

namespace BubbleTeaShop.Backend.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IAssignmentHistoryRepository _assignmentHistoryRepository;
    private readonly IShiftRepository _shiftRepository;

    public EmployeeService(
        IEmployeeRepository employeeRepository,
        IAssignmentHistoryRepository assignmentHistoryRepository,
        IShiftRepository shiftRepository)
    {
        _employeeRepository = employeeRepository;
        _assignmentHistoryRepository = assignmentHistoryRepository;
        _shiftRepository = shiftRepository;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto> GetEmployeeByIdAsync(int id)
    {
        var e = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (e == null) throw new KeyNotFoundException("Employee with given ID does not exist.");
        return MapToDto(e);
    }

    public async Task<int> CreateEmployeeAsync(AddEmployeeDto dto)
    {
        ValidatePhone(dto.Phone);
        ValidateOptionalRanges(dto.HygieneScore, dto.RegisterProficiency);

        var roles = dto.Roles ?? new List<EmployeeRole>();
        if (dto.HygieneScore.HasValue && !roles.Contains(EmployeeRole.BARISTA))
            throw new InvalidOperationException("HygieneScore may be set only for BARISTA role.");
        if (dto.RegisterProficiency.HasValue && !roles.Contains(EmployeeRole.CASHIER))
            throw new InvalidOperationException("RegisterProficiency may be set only for CASHIER role.");
        if (dto.TrainingSessionsConducted.HasValue && !roles.Contains(EmployeeRole.MANAGER))
            throw new InvalidOperationException("TrainingSessionsConducted may be set only for MANAGER role.");
        if (dto.Certificates?.Any() == true && !roles.Contains(EmployeeRole.BARISTA))
            throw new InvalidOperationException("Certificates can be provided only for BARISTA role.");

        if (dto.AssignmentHistories == null || !dto.AssignmentHistories.Any())
            throw new ArgumentException("Employee must have at least one AssignmentHistory.", nameof(dto.AssignmentHistories));

        foreach (var ah in dto.AssignmentHistories)
        {
            if (ah.StartDate >= ah.EndDate)
                throw new ArgumentException("AssignmentHistory StartDate must be before EndDate.");
        }

        var employee = new Employee
        {
            FullName = dto.FullName,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            Salary = dto.Salary,
        };

        if (roles.Any())
        {
            employee.EmployeeRoles = roles
                .Distinct()
                .Select(r => new EmployeeRoleMapping { Role = r })
                .ToList();
        }

        if (dto.HygieneScore.HasValue) employee.HygieneScore = dto.HygieneScore.Value;
        if (dto.RegisterProficiency.HasValue) employee.RegisterProficiency = dto.RegisterProficiency.Value;
        if (dto.TrainingSessionsConducted.HasValue) employee.TrainingSessionsConducted = dto.TrainingSessionsConducted.Value;

        if (dto.Certificates?.Any() == true)
        {
            employee.EmployeeCertificates = dto.Certificates
                .Select(c => new EmployeeCertificate { CertificateName = c })
                .ToList();
        }

        await _employeeRepository.AddEmployeeAsync(employee);
        
        foreach (var ahDto in dto.AssignmentHistories)
        {
            var ah = new AssignmentHistory
            {
                StoreId = ahDto.StoreId,
                EmployeeId = employee.Id,
                StartDate = ahDto.StartDate,
                EndDate = ahDto.EndDate
            };
            await _assignmentHistory_repository_add(ah);
        }

        return employee.Id;
    }

    public async Task UpdateEmployeeAsync(UpdateEmployeeDto dto)
    {
        var existing = await _employeeRepository.GetEmployeeByIdAsync(dto.Id);
        if (existing == null)
            throw new KeyNotFoundException("Employee with given ID does not exist.");

        ValidatePhone(dto.Phone);
        ValidateOptionalRanges(dto.HygieneScore, dto.RegisterProficiency);

        var newRoles = (dto.Roles != null && dto.Roles.Any())
            ? dto.Roles.Distinct().ToList()
            : existing.EmployeeRoles?.Select(rm => rm.Role).ToList() ?? new List<EmployeeRole>();

        if (dto.HygieneScore.HasValue && !newRoles.Contains(EmployeeRole.BARISTA))
            throw new InvalidOperationException("HygieneScore may be set only for BARISTA role.");
        if (dto.RegisterProficiency.HasValue && !newRoles.Contains(EmployeeRole.CASHIER))
            throw new InvalidOperationException("RegisterProficiency may be set only for CASHIER role.");
        if (dto.TrainingSessionsConducted.HasValue && !newRoles.Contains(EmployeeRole.MANAGER))
            throw new InvalidOperationException("TrainingSessionsConducted may be set only for MANAGER role.");
        if (dto.Certificates?.Any() == true && !newRoles.Contains(EmployeeRole.BARISTA))
            throw new InvalidOperationException("Certificates can be provided only for BARISTA role.");

        existing.FullName = dto.FullName;
        existing.Phone = dto.Phone;
        existing.Email = dto.Email;
        existing.Address = dto.Address;
        existing.Salary = dto.Salary;

        if (dto.Roles != null)
        {
            existing.EmployeeRoles = dto.Roles
                .Distinct()
                .Select(r => new EmployeeRoleMapping { EmployeeId = existing.Id, Role = r })
                .ToList();
        }

        if (dto.HygieneScore.HasValue) existing.HygieneScore = dto.HygieneScore.Value;
        if (dto.RegisterProficiency.HasValue) existing.RegisterProficiency = dto.RegisterProficiency.Value;
        if (dto.TrainingSessionsConducted.HasValue) existing.TrainingSessionsConducted = dto.TrainingSessionsConducted.Value;

        if (dto.Certificates != null)
        {
            existing.EmployeeCertificates = dto.Certificates.Select(c => new EmployeeCertificate { CertificateName = c }).ToList();
        }

        await _employeeRepository.UpdateEmployeeAsync(existing);
    }

    public async Task DeleteEmployeeAsync(int id)
    {
        if (!await _employeeRepository.EmployeeExistsAsync(id))
            throw new KeyNotFoundException("Employee with given ID does not exist.");

        var histories = await _assignmentHistoryRepository.GetAssignmentHistoriesByEmployeeIdAsync(id);
        if (histories.Any())
            throw new InvalidOperationException("Cannot delete employee who still has assignment histories.");

        var shifts = await _shiftRepository.GetAllShiftsAsync();
        if (shifts.Any(s => s.ManagingEmployeeId == id))
            throw new InvalidOperationException("Cannot delete employee who manages one or more shifts.");

        await _employeeRepository.DeleteEmployeeAsync(id);
    }

    public async Task AssignEmployeeToShiftAsync(int employeeId, int shiftId)
    {
        if (!await _employeeRepository.EmployeeExistsAsync(employeeId))
            throw new KeyNotFoundException("Employee does not exist.");
        if (!await _shift_repository_check_exists(shiftId))
            throw new KeyNotFoundException("Shift does not exist.");

        await _shiftRepository.AddEmployeeToShiftAsync(shiftId, employeeId);
    }

    public async Task RemoveEmployeeFromShiftAsync(int employeeId, int shiftId)
    {
        var shift = await _shiftRepository.GetShiftByIdAsync(shiftId);
        if (shift == null) throw new KeyNotFoundException("Shift does not exist.");
        if (shift.ManagingEmployeeId == employeeId)
            throw new InvalidOperationException("Cannot remove an employee who is managing this shift.");

        await _shiftRepository.RemoveEmployeeFromShiftAsync(shiftId, employeeId);
    }

    public async Task AssignManagerToShiftAsync(int employeeId, int shiftId)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        if (employee == null) throw new KeyNotFoundException("Employee does not exist.");
        var hasManagerRole = (employee.EmployeeRoles?.Any(rm => rm.Role == EmployeeRole.MANAGER)) ?? false;
        if (!hasManagerRole) throw new InvalidOperationException("Employee must have MANAGER role to manage a shift.");

        await _shiftRepository.AssignManagerToShiftAsync(shiftId, employeeId);
    }

    public async Task RemoveManagerFromShiftAsync(int shiftId)
    {
        await _shiftRepository.RemoveManagerFromShiftAsync(shiftId);
    }
    
    private void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length != 9 || !phone.All(char.IsDigit))
            throw new ArgumentException("Phone number must contain exactly 9 digits.");
    }

    private void ValidateOptionalRanges(int? hygieneScore, int? registerProficiency)
    {
        if (hygieneScore.HasValue && (hygieneScore < 0 || hygieneScore > 10))
            throw new ArgumentException("HygieneScore must be between 0 and 10.");
        if (registerProficiency.HasValue && (registerProficiency < 0 || registerProficiency > 10))
            throw new ArgumentException("RegisterProficiency must be between 0 and 10.");
    }

    private EmployeeDto MapToDto(Employee e)
    {
        return new EmployeeDto
        {
            Id = e.Id,
            FullName = e.FullName,
            Phone = e.Phone,
            Email = e.Email,
            Address = e.Address,
            Salary = e.Salary,
            HygieneScore = e.HygieneScore,
            RegisterProficiency = e.RegisterProficiency,
            TrainingSessionsConducted = e.TrainingSessionsConducted,
            Roles = e.EmployeeRoles?.Select(rm => rm.Role).ToList() ?? new List<EmployeeRole>(),
            Certificates = e.EmployeeCertificates?.Select(c => c.CertificateName).ToList() ?? new List<string>(),
            AssignmentHistories = e.AssignmentHistories?.Select(ah => new AssignmentHistoryDto
            {
                Id = ah.Id,
                StoreId = ah.StoreId,
                EmployeeId = ah.EmployeeId,
                StartDate = ah.StartDate,
                EndDate = ah.EndDate
            }).ToList() ?? new List<AssignmentHistoryDto>()
        };
    }

    private async Task _assignmentHistory_repository_add(AssignmentHistory ah) =>
        await _assignmentHistoryRepository.AddAssignmentHistoryAsync(ah);

    private async Task<bool> _shift_repository_check_exists(int shiftId) =>
        await _shiftRepository.ShiftExistsAsync(shiftId);
}