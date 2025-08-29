using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTeaShop.Backend.Repositories.MenuItem;

public class MilkTeaMenuItemRepository : IMilkTeaMenuItemRepository
{
    private readonly ApplicationDbContext _context;

    public MilkTeaMenuItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<MilkTea>> GetAllMilkTeasAsync()
    {
        return await _context.MilkTeas
            .Include(mt => mt.MenuItemAllergens)
            .Include(mt => mt.OrderLines)
            .ToListAsync();
    }
    
    public async Task<MilkTea> GetMilkTeaByIdAsync(int id)
    {
        return await _context.MilkTeas
            .Include(mt => mt.MenuItemAllergens)
            .Include(mt => mt.OrderLines)
            .FirstOrDefaultAsync(mt => mt.Id == id);
    }
    
    public async Task AddMilkTeaAsync(MilkTea milkTea)
    {
        await _context.MilkTeas.AddAsync(milkTea);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateMilkTeaAsync(MilkTea milkTea)
    {
        _context.MilkTeas.Update(milkTea);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteMilkTeaAsync(int id)
    {
        var milkTea = await _context.MilkTeas.FirstOrDefaultAsync(mt => mt.Id == id);
        if (milkTea != null)
        {
            _context.MilkTeas.Remove(milkTea);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> MilkTeaExistsAsync(int id)
    {
        return await _context.MilkTeas.AnyAsync(mt => mt.Id == id);
    }
}