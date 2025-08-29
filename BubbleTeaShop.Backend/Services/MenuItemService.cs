using BubbleTeaShop.Backend.DTOs.MenuItemDtos;
using BubbleTeaShop.Backend.Models;
using BubbleTeaShop.Backend.Repositories.MenuItem;

namespace BubbleTeaShop.Backend.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly IMilkTeaMenuItemRepository _milkTeaRepository;
    private readonly IFruitTeaMenuItemRepository _fruitTeaRepository;
    private readonly IFrappeMenuItemRepository _frappeRepository;

    public MenuItemService(
        IMenuItemRepository menuItemRepository,
        IMilkTeaMenuItemRepository milkTeaRepository,
        IFruitTeaMenuItemRepository fruitTeaRepository,
        IFrappeMenuItemRepository frappeRepository)
    {
        _menuItemRepository = menuItemRepository;
        _milkTeaRepository = milkTeaRepository;
        _fruitTeaRepository = fruitTeaRepository;
        _frappeRepository = frappeRepository;
    }
    
    public async Task<IEnumerable<MenuItemDto>> GetAllMenuItemsAsync()
    {
        var items = await _menuItemRepository.GetAllMenuItemsAsync();
        return items.Select(MapToDto).ToList();
    }

    public async Task<MenuItemDto> GetMenuItemByIdAsync(int id)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(id))
            throw new KeyNotFoundException($"MenuItem with id {id} does not exist.");

        var mi = await _menuItemRepository.GetMenuItemByIdAsync(id);
        return MapToDto(mi);
    }

    public async Task<List<MenuItem>> GetMenuItemsByIdsAsync(IEnumerable<int> ids)
    {
        return await _menuItemRepository.GetMenuItemsByIdsAsync(ids);
    }
    
    public async Task<int> AddMilkTeaAsync(AddMilkTeaDto dto)
    {
        ValidateBaseAddDto(dto);
        var entity = new MilkTea
        {
            Name = dto.Name,
            BasePrice = dto.BasePrice,
            StockQuantity = dto.StockQuantity,
            MilkBase = dto.MilkBase,
            TeaBase = dto.TeaBase,
            MenuItemAllergens = dto.Allergens?.Select(a => new MenuItemAllergen { Name = a }).ToList()
        };

        await _milkTeaRepository.AddMilkTeaAsync(entity);
        return entity.Id;
    }

    public async Task<int> AddFruitTeaAsync(AddFruitTeaDto dto)
    {
        ValidateBaseAddDto(dto);
        var entity = new FruitTea
        {
            Name = dto.Name,
            BasePrice = dto.BasePrice,
            StockQuantity = dto.StockQuantity,
            FruitBase = dto.FruitBase,
            TeaBase = dto.TeaBase,
            IceLevel = dto.IceLevel,
            MenuItemAllergens = dto.Allergens?.Select(a => new MenuItemAllergen { Name = a }).ToList()
        };

        await _fruitTeaRepository.AddFruitTeaAsync(entity);
        return entity.Id;
    }

    public async Task<int> AddFrappeAsync(AddFrappeDto dto)
    {
        ValidateBaseAddDto(dto);
        var entity = new Frappe
        {
            Name = dto.Name,
            BasePrice = dto.BasePrice,
            StockQuantity = dto.StockQuantity,
            BaseFlavor = dto.BaseFlavor,
            HasWhippedCream = dto.HasWhippedCream,
            MenuItemAllergens = dto.Allergens?.Select(a => new MenuItemAllergen { Name = a }).ToList()
        };

        await _frappeRepository.AddFrappeAsync(entity);
        return entity.Id;
    }
    
    public async Task UpdateMilkTeaAsync(UpdateMilkTeaDto dto)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(dto.Id))
            throw new KeyNotFoundException($"MenuItem with id {dto.Id} does not exist.");

        var existing = await _milkTeaRepository.GetMilkTeaByIdAsync(dto.Id);
        if (existing == null)
            throw new KeyNotFoundException($"MilkTea with id {dto.Id} does not exist.");

        ApplyBaseUpdates(existing, dto);

        if (dto.TeaBase.HasValue) existing.TeaBase = dto.TeaBase.Value;
        if (dto.MilkBase.HasValue) existing.MilkBase = dto.MilkBase.Value;

        await _milkTeaRepository.UpdateMilkTeaAsync(existing);
    }

    public async Task UpdateFruitTeaAsync(UpdateFruitTeaDto dto)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(dto.Id))
            throw new KeyNotFoundException($"MenuItem with id {dto.Id} does not exist.");

        var existing = await _fruitTeaRepository.GetFruitTeaByIdAsync(dto.Id);
        if (existing == null)
            throw new KeyNotFoundException($"FruitTea with id {dto.Id} does not exist.");

        ApplyBaseUpdates(existing, dto);

        if (dto.FruitBase.HasValue) existing.FruitBase = dto.FruitBase.Value;
        if (dto.TeaBase.HasValue) existing.TeaBase = dto.TeaBase.Value;
        if (dto.IceLevel.HasValue) existing.IceLevel = dto.IceLevel.Value;

        await _fruitTeaRepository.UpdateFruitTeaAsync(existing);
    }

    public async Task UpdateFrappeAsync(UpdateFrappeDto dto)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(dto.Id))
            throw new KeyNotFoundException($"MenuItem with id {dto.Id} does not exist.");

        var existing = await _frappeRepository.GetFrappeByIdAsync(dto.Id);
        if (existing == null)
            throw new KeyNotFoundException($"Frappe with id {dto.Id} does not exist.");

        ApplyBaseUpdates(existing, dto);

        if (dto.BaseFlavor.HasValue) existing.BaseFlavor = dto.BaseFlavor.Value;
        if (dto.HasWhippedCream.HasValue) existing.HasWhippedCream = dto.HasWhippedCream.Value;

        await _frappeRepository.UpdateFrappeAsync(existing);
    }
    
    public async Task DeleteMenuItemAsync(int id)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(id))
            throw new KeyNotFoundException($"MenuItem with id {id} does not exist.");

        var mi = await _menuItemRepository.GetMenuItemByIdAsync(id);
        if (mi == null)
            throw new KeyNotFoundException($"MenuItem with id {id} does not exist.");

        // do not remove menu item that is part of orderline
        if (mi.OrderLines != null && mi.OrderLines.Any())
            throw new InvalidOperationException("Cannot delete MenuItem that is part of existing orders.");

        switch (mi)
        {
            case MilkTea _:
                await _milkTeaRepository.DeleteMilkTeaAsync(id);
                break;
            case FruitTea _:
                await _fruitTeaRepository.DeleteFruitTeaAsync(id);
                break;
            case Frappe _:
                await _frappeRepository.DeleteFrappeAsync(id);
                break;
            default:
                await _menuItemRepository.DeleteMenuItemAsync(id);
                break;
        }
    }
    
    public async Task<bool> IsAvailableAsync(int menuItemId, int requiredQuantity = 1)
    {
        if (requiredQuantity <= 0)
            throw new ArgumentException("requiredQuantity must be > 0", nameof(requiredQuantity));
        if (!await _menuItemRepository.MenuItemExistsAsync(menuItemId))
            throw new KeyNotFoundException($"MenuItem with id {menuItemId} does not exist.");

        var mi = await _menuItemRepository.GetMenuItemByIdAsync(menuItemId);
        return (mi?.StockQuantity ?? 0) >= requiredQuantity;
    }

    public async Task<int> AdjustStockAsync(int menuItemId, int delta)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(menuItemId))
            throw new KeyNotFoundException($"MenuItem with id {menuItemId} does not exist.");

        var mi = await _menuItemRepository.GetMenuItemByIdAsync(menuItemId);
        if (mi == null) throw new KeyNotFoundException($"MenuItem with id {menuItemId} does not exist.");

        var newStock = mi.StockQuantity + delta;
        if (newStock < 0) throw new InvalidOperationException("Stock cannot be negative.");

        mi.StockQuantity = newStock;

        switch (mi)
        {
            case MilkTea mt:
                await _milkTeaRepository.UpdateMilkTeaAsync(mt);
                break;
            case FruitTea ft:
                await _fruitTeaRepository.UpdateFruitTeaAsync(ft);
                break;
            case Frappe f:
                await _frappeRepository.UpdateFrappeAsync(f);
                break;
            default:
                await _menuItemRepository.UpdateMenuItemAsync(mi);
                break;
        }

        return newStock;
    }
    
    private static void ValidateBaseAddDto(AddMenuItemBaseDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.", nameof(dto.Name));
        if (dto.BasePrice < 0) throw new ArgumentException("BasePrice must be >= 0.", nameof(dto.BasePrice));
        if (dto.StockQuantity < 0)
            throw new ArgumentException("StockQuantity must be >= 0.", nameof(dto.StockQuantity));
    }

    private static void ApplyBaseUpdates(MenuItem existing, UpdateMenuItemBaseDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Name)) existing.Name = dto.Name;
        if (dto.BasePrice.HasValue) existing.BasePrice = dto.BasePrice.Value;
        if (dto.StockQuantity.HasValue)
        {
            if (dto.StockQuantity.Value < 0)
                throw new ArgumentException("StockQuantity must be >= 0", nameof(dto.StockQuantity));
            existing.StockQuantity = dto.StockQuantity.Value;
        }

        if (dto.Allergens != null)
        {
            existing.MenuItemAllergens = dto.Allergens.Select(a => new MenuItemAllergen { Name = a }).ToList();
        }
    }

    private static MenuItemDto MapToDto(MenuItem mi)
    {
        if (mi == null) return null;

        var baseAllergens = mi.MenuItemAllergens?.Select(a => a.Name).ToList() ?? new List<string>();
        int orderLineCount = mi.OrderLines?.Count ?? 0;

        switch (mi)
        {
            case MilkTea mt:
                return new MilkTeaDto
                {
                    Id = mt.Id,
                    Name = mt.Name,
                    BasePrice = mt.BasePrice,
                    StockQuantity = mt.StockQuantity,
                    Allergens = baseAllergens,
                    ItemType = nameof(MilkTea),
                    MilkBase = mt.MilkBase,
                    TeaBase = mt.TeaBase
                };

            case FruitTea ft:
                return new FruitTeaDto
                {
                    Id = ft.Id,
                    Name = ft.Name,
                    BasePrice = ft.BasePrice,
                    StockQuantity = ft.StockQuantity,
                    Allergens = baseAllergens,
                    ItemType = nameof(FruitTea),
                    FruitBase = ft.FruitBase,
                    TeaBase = ft.TeaBase,
                    IceLevel = ft.IceLevel
                };

            case Frappe fr:
                return new FrappeDto
                {
                    Id = fr.Id,
                    Name = fr.Name,
                    BasePrice = fr.BasePrice,
                    StockQuantity = fr.StockQuantity,
                    Allergens = baseAllergens,
                    ItemType = nameof(Frappe),
                    BaseFlavor = fr.BaseFlavor,
                    HasWhippedCream = fr.HasWhippedCream
                };

            default:
                return new MenuItemDto
                {
                    Id = mi.Id,
                    Name = mi.Name,
                    BasePrice = mi.BasePrice,
                    StockQuantity = mi.StockQuantity,
                    Allergens = baseAllergens,
                    ItemType = mi.GetType().Name
                };
        }
    }
}