using BubbleTeaShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IAssignmentHistoryRepository
{
    Task<IEnumerable<AssignmentHistory>> GetAllAssignmentHistoriesAsync();
    Task<AssignmentHistory> GetAssignmentHistoryByIdAsync(int storeId, int employeeId);
    Task AddAssignmentHistoryAsync(AssignmentHistory assignmentHistory);
    Task UpdateAssignmentHistoryAsync(AssignmentHistory assignmentHistory);
    Task DeleteAssignmentHistoryAsync(int storeId, int employeeId);
    Task<bool> AssignmentHistoryExistsAsync(int storeId, int employeeId);
    Task<IEnumerable<AssignmentHistory>> GetAssignmentHistoriesByStoreIdAsync(int storeId);
    Task<IEnumerable<AssignmentHistory>> GetAssignmentHistoriesByEmployeeIdAsync(int employeeId);
}