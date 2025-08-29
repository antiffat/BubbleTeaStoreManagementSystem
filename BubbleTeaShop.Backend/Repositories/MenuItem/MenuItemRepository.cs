using BubbleTeaShop.Backend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BubbleTeaShop.Backend.Repositories.MenuItem;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly ApplicationDbContext _context;
    
    public MenuItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Models.MenuItem>> GetAllMenuItemsAsync()
    {
        return await _context.MenuItems
            .Include(mi => mi.MenuItemAllergens)
            .Include(mi => mi.OrderLines)
            .ToListAsync();
    }
    
    public async Task<Models.MenuItem> GetMenuItemByIdAsync(int id)
    {
        return await _context.MenuItems
            .Include(mi => mi.MenuItemAllergens)
            .Include(mi => mi.OrderLines)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task AddMenuItemAsync(Models.MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateMenuItemAsync(Models.MenuItem menuItem)
    {
        _context.MenuItems.Update(menuItem);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteMenuItemAsync(int id)
    {
        var menuItem = await _context.MenuItems.FirstOrDefaultAsync(mi => mi.Id == id);
        if (menuItem != null)
        {
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> MenuItemExistsAsync(int id)
    {
        return await _context.MenuItems.AnyAsync(mi => mi.Id == id);
    }
    
    public async Task<List<Models.MenuItem>> GetMenuItemsByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        return await _context.MenuItems
            .Where(mi => idList.Contains(mi.Id))
            .ToListAsync();
    }
}