using BubbleTeaShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IFruitTeaMenuItemRepository
{
    Task<IEnumerable<FruitTea>> GetAllFruitTeasAsync();
    Task<FruitTea> GetFruitTeaByIdAsync(int id);
    Task AddFruitTeaAsync(FruitTea fruitTea);
    Task UpdateFruitTeaAsync(FruitTea fruitTea);
    Task DeleteFruitTeaAsync(int id);
    Task<bool> FruitTeaExistsAsync(int id);
}