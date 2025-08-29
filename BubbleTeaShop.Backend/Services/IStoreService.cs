using BubbleTeaShop.Backend.DTOs.StoreDto;

namespace BubbleTeaShop.Backend.Services;

public interface IStoreService
{
    Task<IEnumerable<StoreDto>> GetAllStoresAsync();
    Task<StoreDto> GetStoreByIdAsync(int id);
    Task<int> AddStoreAsync(AddStoreDto dto);
    Task UpdateStoreAsync(UpdateStoreDto dto);
    Task DeleteStoreAsync(int id);
}