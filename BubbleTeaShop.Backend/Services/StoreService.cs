using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.DTOs.MenuItemDtos;
using BubbleTesShop.Backend.DTOs.StoreDto;
using BubbleTesShop.Backend.Repositories;

namespace BubbleTesShop.Backend.Services;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;

    public StoreService(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<IEnumerable<StoreDto>> GetAllStoresAsync()
    {
        var stores = await _storeRepository.GetAllStoresAsync();
        return stores.Select(MapToDto);
    }

    public async Task<StoreDto> GetStoreByIdAsync(int id)
    {
        var store = await _storeRepository.GetStoreByIdAsync(id);
        if (store == null)
            throw new KeyNotFoundException($"Store with id {id} not found.");

        return MapToDto(store);
    }

    public async Task<int> AddStoreAsync(AddStoreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("Name is required.", nameof(dto.Name));
        if (string.IsNullOrWhiteSpace(dto.Location))
            throw new ArgumentException("Location is required.", nameof(dto.Location));

        var store = new Store
        {
            Name = dto.Name,
            Location = dto.Location
        };

        await _storeRepository.AddStoreAsync(store);
        return store.Id;
    }

    public async Task UpdateStoreAsync(UpdateStoreDto dto)
    {
        var store = await _storeRepository.GetStoreByIdAsync(dto.Id);
        if (store == null)
            throw new KeyNotFoundException($"Store with id {dto.Id} not found.");

        if (!string.IsNullOrWhiteSpace(dto.Name))
            store.Name = dto.Name;
        if (!string.IsNullOrWhiteSpace(dto.Location))
            store.Location = dto.Location;

        await _storeRepository.UpdateStoreAsync(store);
    }

    public async Task DeleteStoreAsync(int id)
    {
        var store = await _storeRepository.GetStoreByIdAsync(id);
        if (store == null)
            throw new KeyNotFoundException($"Store with id {id} not found.");

        if (store.MenuItems != null && store.MenuItems.Any())
            throw new InvalidOperationException("Cannot delete store that has associated menu items.");

        await _storeRepository.DeleteStoreAsync(id);
    }
    
    private static StoreDto MapToDto(Store store)
    {
        return new StoreDto
        {
            Id = store.Id,
            Name = store.Name,
            Location = store.Location
        };
    }
}