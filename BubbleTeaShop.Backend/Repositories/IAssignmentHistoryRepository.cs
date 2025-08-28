using BubbleTeaShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IAssignmentHistoryRepository
{
    Task<IEnumerable<AssignmentHistory>> GetAllAssignmentHistoriesAsync();
    Task<AssignmentHistory> GetAssignmentHistoryByIdAsync(int id);
    Task AddAssignmentHistoryAsync(AssignmentHistory assignmentHistory);
    Task UpdateAssignmentHistoryAsync(AssignmentHistory assignmentHistory);
    Task DeleteAssignmentHistoryAsync(int id);
    Task<bool> AssignmentHistoryExistsAsync(int id);
    Task<IEnumerable<AssignmentHistory>> GetAssignmentHistoriesByStoreIdAsync(int storeId);
    Task<IEnumerable<AssignmentHistory>> GetAssignmentHistoriesByEmployeeIdAsync(int employeeId);
    Task<bool> TryDeleteAssignmentHistoryIfEmployeeHasMoreThanOneAsync(int assignmentHistoryId);
}