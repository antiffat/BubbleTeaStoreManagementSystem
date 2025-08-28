
using BubbleTesShop.Backend.DTOs.AssignmentHistoryDtos;

namespace BubbleTesShop.Backend.Services;

public interface IAssignmentHistoryService
{
    Task<IEnumerable<AssignmentHistoryDto>> GetAllAssignmentHistoriesAsync();
    Task<AssignmentHistoryDto> GetAssignmentHistoryByIdAsync(int id);
    Task<int> AddAssignmentHistoryAsync(AddAssignmentHistoryDto assignmentHistoryDto);
    Task UpdateAssignmentHistoryAsync(UpdateAssignmentHistoryDto assignmentHistoryDto);
    Task DeleteAssignmentHistoryAsync(int id);
    Task<IEnumerable<AssignmentHistoryDto>> GetAssignmentHistoriesByStoreIdAsync(int storeId);
    Task<IEnumerable<AssignmentHistoryDto>> GetAssignmentHistoriesByEmployeeIdAsync(int employeeId);
}