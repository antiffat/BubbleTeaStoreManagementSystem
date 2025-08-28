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
    
    public async Task<AssignmentHistory> GetAssignmentHistoryByIdAsync(int id)
    {
        return await _context.AssignmentHistories
            .Include(ah => ah.Store)
            .Include(ah => ah.Employee)
            .FirstOrDefaultAsync(ol => ol.Id == id);
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
    
    public async Task DeleteAssignmentHistoryAsync(int id)
    {
        var ah = await _context.AssignmentHistories.FirstOrDefaultAsync(ah => ah.Id == id);
        if (ah != null)
        {
            _context.AssignmentHistories.Remove(ah);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> AssignmentHistoryExistsAsync(int id)
    {
        return await _context.AssignmentHistories.AnyAsync(ah => ah.Id == id);
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

    public async Task<bool> TryDeleteAssignmentHistoryIfEmployeeHasMoreThanOneAsync(int assignmentHistoryId)
    {
        using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var ah = await _context.AssignmentHistories
            .Where(x => x.Id == assignmentHistoryId)
            .Include(x => x.Employee)
            .ThenInclude(o => o.AssignmentHistories)
            .FirstOrDefaultAsync();

        if (ah == null) return false;

        if (ah.Employee == null)
            throw new InvalidOperationException("Data integrity error: parent employee missing");

        if (ah.Employee.AssignmentHistories?.Count <= 1)
            throw new InvalidOperationException("Cannot delete last assignment history of an employee.");

        _context.AssignmentHistories.Remove(ah);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return true;
    }
}