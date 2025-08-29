using BubbleTeaShop.Backend.Models;

namespace BubbleTeaShop.Backend.Repositories.MenuItem;

public interface IFrappeMenuItemRepository
{
    Task<IEnumerable<Frappe>> GetAllFrappesAsync();
    Task<Frappe> GetFrappeByIdAsync(int id);
    Task AddFrappeAsync(Frappe frappe);
    Task UpdateFrappeAsync(Frappe frappe);
    Task DeleteFrappeAsync(int id);
    Task<bool> FrappeExistsAsync(int id);
}