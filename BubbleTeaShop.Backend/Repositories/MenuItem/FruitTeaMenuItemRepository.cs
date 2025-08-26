using BubbleTeaShop.Backend.Helpers;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Repositories;

public class FruitTeaMenuItemRepository : IFruitTeaMenuItemRepository
{
    private readonly ApplicationDbContext _context;

    public FruitTeaMenuItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<FruitTea>> GetAllFruitTeasAsync()
    {
        return await _context.FruitTeas
            .Include(ft => ft.MenuItemAllergens)
            .Include(ft => ft.OrderLines)
            .ToListAsync();
    }
    
    public async Task<FruitTea> GetFruitTeaByIdAsync(int id)
    {
        return await _context.FruitTeas
            .Include(ft => ft.MenuItemAllergens)
            .Include(ft => ft.OrderLines)
            .FirstOrDefaultAsync(ft => ft.Id == id);
    }
    
    public async Task AddFruitTeaAsync(FruitTea fruitTea)
    {
        await _context.FruitTeas.AddAsync(fruitTea);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateFruitTeaAsync(FruitTea fruitTea)
    {
        _context.FruitTeas.Update(fruitTea);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteFruitTeaAsync(int id)
    {
        var fruitTea = await _context.FruitTeas.FirstOrDefaultAsync(ft => ft.Id == id);
        if (fruitTea != null)
        {
            _context.FruitTeas.Remove(fruitTea);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> FruitTeaExistsAsync(int id)
    {
        return await _context.FruitTeas.AnyAsync(ft => ft.Id == id);
    }
}