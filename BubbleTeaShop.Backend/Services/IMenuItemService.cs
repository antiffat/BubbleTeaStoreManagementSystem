using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.DTOs.MenuItemDtos;

namespace BubbleTesShop.Backend.Services;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync();
    Task<MenuItemDto> GetMenuItemByIdAsync(int id);
    Task<List<MenuItem>> GetMenuItemsByIdsAsync(IEnumerable<int> ids);
    Task<int> AddMilkTeaAsync(AddMilkTeaDto dto);
    Task<int> AddFruitTeaAsync(AddFruitTeaDto dto);
    Task<int> AddFrappeAsync(AddFrappeDto dto);
    Task UpdateMilkTeaAsync(UpdateMilkTeaDto dto);
    Task UpdateFruitTeaAsync(UpdateFruitTeaDto dto);
    Task UpdateFrappeAsync(UpdateFrappeDto dto);
    Task DeleteMenuItemAsync(int id);
    Task<bool> IsAvailableAsync(int menuItemId, int requiredQuantity = 1);
    Task<int> AdjustStockAsync(int menuItemId, int delta);
}