using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTeaShop.Backend.Repositories;

public class StoreRepository : IStoreRepository
{
    private readonly ApplicationDbContext _context;

    public StoreRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Store>> GetAllStoresAsync()
    {
        return await _context.Stores
            .Include(s => s.MenuItems)
            .ToListAsync();
    }
    
    public async Task<Store> GetStoreByIdAsync(int id)
    {
        return await _context.Stores
            .Include(s => s.MenuItems)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task AddStoreAsync(Store store)
    {
        await _context.Stores.AddAsync(store);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateStoreAsync(Store store)
    {
        _context.Stores.Update(store);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteStoreAsync(int id)
    {
        var store = await _context.Stores.FirstOrDefaultAsync(s => s.Id == id);
        if (store != null)
        {
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> StoreExistsAsync(int id)
    {
        return await _context.Stores.AnyAsync(s => s.Id == id);
    }
}