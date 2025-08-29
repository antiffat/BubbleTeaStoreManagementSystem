namespace BubbleTeaShop.Backend.Repositories.MenuItem;

public interface IMenuItemRepository
{
    Task<IEnumerable<Models.MenuItem>> GetAllMenuItemsAsync();
    Task<Models.MenuItem> GetMenuItemByIdAsync(int id);
    Task AddMenuItemAsync(Models.MenuItem menuItem);
    Task UpdateMenuItemAsync(Models.MenuItem menuItem);
    Task DeleteMenuItemAsync(int id);
    Task<bool> MenuItemExistsAsync(int id);
    Task<List<Models.MenuItem>> GetMenuItemsByIdsAsync(IEnumerable<int> ids);
}