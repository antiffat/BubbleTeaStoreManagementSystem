using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Repositories;

public class AssignmentHistoryRepository : IAssignmentHistoryRepository
{
    private readonly ApplicationDbContext _context;

    public AssignmentHistoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<AssignmentHistory>> GetAllAssignmentHistoriesAsync()
    {
        return await _context.AssignmentHistories
            .Include(ah => ah.Store)
            .Include(ah => ah.Employee)
            .ToListAsync();
    }
    
    public async Task<AssignmentHistory> GetAssignmentHistoryByIdAsync(int storeId, int employeeId)
    {
        return await _context.AssignmentHistories
            .Include(ah => ah.Store)
            .Include(ah => ah.Employee)
            .FirstOrDefaultAsync(ah => ah.StoreId == storeId && ah.EmployeeId == employeeId);
    }
    
    public async Task AddAssignmentHistoryAsync(AssignmentHistory assignmentHistory)
    {
        await _context.AssignmentHistories.AddAsync(assignmentHistory);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAssignmentHistoryAsync(AssignmentHistory assignmentHistory)
    {
        _context.AssignmentHistories.Update(assignmentHistory);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAssignmentHistoryAsync(int storeId, int employeeId)
    {
        var assignmentHistory = await _context.AssignmentHistories.FirstOrDefaultAsync(ah => ah.StoreId == storeId && ah.EmployeeId == employeeId);
        if (assignmentHistory != null)
        {
            _context.AssignmentHistories.Remove(assignmentHistory);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> AssignmentHistoryExistsAsync(int storeId, int employeeId)
    {
        return await _context.AssignmentHistories.AnyAsync(ah => ah.StoreId == storeId && ah.EmployeeId == employeeId);
    }
    
    public async Task<IEnumerable<AssignmentHistory>> GetAssignmentHistoriesByStoreIdAsync(int storeId)
    {
        return await _context.AssignmentHistories
            .Where(ah => ah.StoreId == storeId)
            .Include(ah => ah.Store)
            .Include(ah => ah.Employee)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<AssignmentHistory>> GetAssignmentHistoriesByEmployeeIdAsync(int employeeId)
    {
        return await _context.AssignmentHistories
            .Where(ah => ah.EmployeeId == employeeId)
            .Include(ah => ah.Store)
            .Include(ah => ah.Employee)
            .ToListAsync();
    }
}