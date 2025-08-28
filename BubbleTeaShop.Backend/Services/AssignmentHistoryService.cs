using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.DTOs.AssignmentHistoryDtos;
using BubbleTesShop.Backend.Repositories;

namespace BubbleTesShop.Backend.Services;

public class AssignmentHistoryService : IAssignmentHistoryService
{
    private readonly IAssignmentHistoryRepository _assignmentHistoryRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public AssignmentHistoryService(IAssignmentHistoryRepository assignmentHistoryRepository,
        IStoreRepository storeRepository,
        IEmployeeRepository employeeRepository)
    {
        _assignmentHistoryRepository = assignmentHistoryRepository;
        _storeRepository = storeRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<AssignmentHistoryDto>> GetAllAssignmentHistoriesAsync()
    {
        var list = await _assignmentHistoryRepository.GetAllAssignmentHistoriesAsync();
        return list.Select(MapToDto);
    }

    public async Task<AssignmentHistoryDto> GetAssignmentHistoryByIdAsync(int id)
    {
        if (!await _assignmentHistoryRepository.AssignmentHistoryExistsAsync(id))
            throw new KeyNotFoundException("Assignment history with given ID does not exist.");

        var ah = await _assignmentHistoryRepository.GetAssignmentHistoryByIdAsync(id);
        return MapToDto(ah);
    }

    public async Task<int> AddAssignmentHistoryAsync(AddAssignmentHistoryDto assignmentHistoryDto)
    {
        if (!await _storeRepository.StoreExistsAsync(assignmentHistoryDto.StoreId))
            throw new KeyNotFoundException("Store with given ID does not exist.");
        if (!await _employeeRepository.EmployeeExistsAsync(assignmentHistoryDto.EmployeeId))
            throw new KeyNotFoundException("Employee with given ID does not exist.");

        if (assignmentHistoryDto.StartDate > assignmentHistoryDto.EndDate)
            throw new ArgumentException("Start date must be before end date. ");

        var assignmentHistory = new AssignmentHistory
        {
            StoreId = assignmentHistoryDto.StoreId,
            EmployeeId = assignmentHistoryDto.EmployeeId,
            StartDate = assignmentHistoryDto.StartDate,
            EndDate = assignmentHistoryDto.EndDate,
        };

        await _assignmentHistoryRepository.AddAssignmentHistoryAsync(assignmentHistory);

        return assignmentHistory.Id;
    }

    public async Task UpdateAssignmentHistoryAsync(UpdateAssignmentHistoryDto assignmentHistoryDto)
    {
        if (!await _assignmentHistoryRepository.AssignmentHistoryExistsAsync(assignmentHistoryDto.Id))
            throw new KeyNotFoundException("Assignment history with given ID does not exist.");

        var existing = await _assignmentHistoryRepository.GetAssignmentHistoryByIdAsync(assignmentHistoryDto.Id);
        if (existing == null)
            throw new KeyNotFoundException("Assignment history with given ID does not exist.");

        if (assignmentHistoryDto.StartDate > assignmentHistoryDto.EndDate)
            throw new ArgumentException("Start date must be before end date. ");

        existing.StartDate = assignmentHistoryDto.StartDate;
        existing.EndDate = assignmentHistoryDto.EndDate;

        await _assignmentHistoryRepository.UpdateAssignmentHistoryAsync(existing);
    }

    public async Task DeleteAssignmentHistoryAsync(int id)
    {
        var deleted = await _assignmentHistoryRepository.TryDeleteAssignmentHistoryIfEmployeeHasMoreThanOneAsync(id);

        if (!deleted)
            throw new KeyNotFoundException("Assignment history with given ID does not exist.");
    }

    public async Task<IEnumerable<AssignmentHistoryDto>> GetAssignmentHistoriesByStoreIdAsync(int storeId)
    {
        if (!await _storeRepository.StoreExistsAsync(storeId))
            throw new KeyNotFoundException("Store with given ID does not exist.");

        var list = await _assignmentHistoryRepository.GetAssignmentHistoriesByStoreIdAsync(storeId);
        return list.Select(MapToDto);
    }

    public async Task<IEnumerable<AssignmentHistoryDto>> GetAssignmentHistoriesByEmployeeIdAsync(int employeeId)
    {
        if (!await _employeeRepository.EmployeeExistsAsync(employeeId))
            throw new KeyNotFoundException("Employee with given ID does not exist.");

        var list = await _assignmentHistoryRepository.GetAssignmentHistoriesByEmployeeIdAsync(employeeId);
        return list.Select(MapToDto);
    }
    private AssignmentHistoryDto MapToDto(AssignmentHistory ah)
    {
        var dto = new AssignmentHistoryDto()
        {
            Id = ah.Id,
            StoreId = ah.StoreId,
            EmployeeId = ah.EmployeeId,
            StartDate = ah.StartDate,
            EndDate = ah.EndDate,
        };
        return dto;
    }
}