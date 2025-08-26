using BubbleTeaShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetAllStoresAsync();
    Task<Store> GetStoreByIdAsync(int id);
    Task AddStoreAsync(Store store);
    Task UpdateStoreAsync(Store store);
    Task DeleteStoreAsync(int id);
    Task<bool> StoreExistsAsync(int id);
}