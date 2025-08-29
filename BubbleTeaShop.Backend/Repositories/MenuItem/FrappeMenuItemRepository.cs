using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTeaShop.Backend.Repositories.MenuItem;

public class FrappeMenuItemRepository : IFrappeMenuItemRepository
{
    private readonly ApplicationDbContext _context;

    public FrappeMenuItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Frappe>> GetAllFrappesAsync()
    {
        return await _context.Frappes
            .Include(f => f.MenuItemAllergens)
            .Include(f => f.OrderLines)
            .ToListAsync();
    }
    
    public async Task<Frappe> GetFrappeByIdAsync(int id)
    {
        return await _context.Frappes
            .Include(f => f.MenuItemAllergens)
            .Include(f => f.OrderLines)
            .FirstOrDefaultAsync(f => f.Id == id);
    }
    
    public async Task AddFrappeAsync(Frappe frappe)
    {
        await _context.Frappes.AddAsync(frappe);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateFrappeAsync(Frappe frappe)
    {
        _context.Frappes.Update(frappe);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteFrappeAsync(int id)
    {
        var frappe = await _context.Frappes.FirstOrDefaultAsync(f => f.Id == id);
        if (frappe != null)
        {
            _context.Frappes.Remove(frappe);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> FrappeExistsAsync(int id)
    {
        return await _context.Frappes.AnyAsync(f => f.Id == id);
    }
}