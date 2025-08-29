using BubbleTeaShop.Backend.Models;

namespace BubbleTeaShop.Backend.Repositories.MenuItem;

public interface IMilkTeaMenuItemRepository
{
    Task<IEnumerable<MilkTea>> GetAllMilkTeasAsync();
    Task<MilkTea> GetMilkTeaByIdAsync(int id);
    Task AddMilkTeaAsync(MilkTea milkTea);
    Task UpdateMilkTeaAsync(MilkTea milkTea);
    Task DeleteMilkTeaAsync(int id);
    Task<bool> MilkTeaExistsAsync(int id);
}