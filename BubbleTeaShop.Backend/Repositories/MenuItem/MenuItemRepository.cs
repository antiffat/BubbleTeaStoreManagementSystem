using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Repositories;

public class MenuItemRepository : IMenuItemRepository
{
    private readonly ApplicationDbContext _context;
    
    public MenuItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<MenuItem>> GetAllMenuItemsAsync()
    {
        return await _context.MenuItems
            .Include(mi => mi.MenuItemAllergens)
            .Include(mi => mi.OrderLines)
            .ToListAsync();
    }
    
    public async Task<MenuItem> GetMenuItemByIdAsync(int id)
    {
        return await _context.MenuItems
            .Include(mi => mi.MenuItemAllergens)
            .Include(mi => mi.OrderLines)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task AddMenuItemAsync(MenuItem menuItem)
    {
        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateMenuItemAsync(MenuItem menuItem)
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
    
    public async Task<List<MenuItem>> GetMenuItemsByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids.Distinct().ToList();
        return await _context.MenuItems
            .Where(mi => idList.Contains(mi.Id))
            .ToListAsync();
    }
}