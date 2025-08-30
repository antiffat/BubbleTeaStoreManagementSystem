using System.Collections.ObjectModel;
using System.Diagnostics;
using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Size = BubbleTeaShop.Backend.Enums.Size;

namespace BubbleTeaShop.Client.ViewModels;

[QueryProperty(nameof(MenuItemId), "MenuItemId")]
[QueryProperty(nameof(MenuItemName), "MenuItemName")]
[QueryProperty(nameof(BasePrice), "BasePrice")]
public partial class CustomizeOrderLineViewModel : ObservableObject
{
    private readonly IMenuItemService _menuItemService;
    private static List<AddOrderLineDto> _currentOrderItems = new();

    [ObservableProperty] private int _menuItemId;
    [ObservableProperty] private string _menuItemName;
    [ObservableProperty] private double _basePrice;
    [ObservableProperty] private int _quantity = 1;
    [ObservableProperty] private Size _selectedSize = Size.S;

    public ObservableCollection<ToppingSelectionItem> AvailableToppings { get; } = new()
    {
        new ToppingSelectionItem { Topping = Topping.TAPIOCA },
        new ToppingSelectionItem { Topping = Topping.KONJAC }
    };

    public ObservableCollection<Size> AvailableSizes { get; } = new()
    {
        Size.S, Size.M, Size.L
    };

    public IRelayCommand CancelCommand { get; }
    public IRelayCommand AddMoreItemsCommand { get; }
    public IRelayCommand FinalizeOrderCommand { get; }

    public CustomizeOrderLineViewModel(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
        CancelCommand = new RelayCommand(ExecuteCancel);
        AddMoreItemsCommand = new RelayCommand(ExecuteAddMoreItems);
        FinalizeOrderCommand = new RelayCommand(ExecuteFinalizeOrder);
    }

    public static IReadOnlyList<AddOrderLineDto> GetCurrentOrderItems() => _currentOrderItems.AsReadOnly();
    public static void ClearCurrentOrderItems() => _currentOrderItems.Clear();

    private async void ExecuteCancel()
    {
        ClearCurrentOrderItems();
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void ExecuteAddMoreItems()
    {
        if (!ValidateQuantityAndMenuItem()) return;

        if (!await CheckStockAvailabilityAsync()) return;

        AddCurrentSelectionToOrder();

        await DisplayAlert("Item Added", $"{Quantity}x {MenuItemName} added to your order!", "OK");
        await Shell.Current.GoToAsync("SelectCategoryPage");
    }

    private async void ExecuteFinalizeOrder()
    {
        if (!ValidateQuantityAndMenuItem()) return;

        if (!await CheckStockAvailabilityAsync()) return;

        AddCurrentSelectionToOrder();

        if (_currentOrderItems.Count == 0)
        {
            await DisplayAlert("No Items", "Your order has no items.", "OK");
            return;
        }

        await Shell.Current.GoToAsync("OrderDetailsSummaryPage");
    }

    private bool ValidateQuantityAndMenuItem()
    {
        if (MenuItemId <= 0)
        {
            _ = DisplayAlert("Error", "No menu item selected.", "OK");
            return false;
        }
        if (Quantity <= 0)
        {
            _ = DisplayAlert("Invalid Quantity", "Quantity must be greater than 0.", "OK");
            return false;
        }
        return true;
    }

    private async Task<bool> CheckStockAvailabilityAsync()
    {
        try
        {
            bool isAvailable = await _menuItemService.IsAvailableAsync(MenuItemId, Quantity);
            if (!isAvailable)
            {
                var menuItem = await _menuItemService.GetMenuItemByIdAsync(MenuItemId);
                await DisplayAlert("Out of Stock", $"Only {menuItem.StockQuantity} of {MenuItemName} are available.", "OK");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking availability: {ex.Message}");
            await DisplayAlert("Error", "Could not check item availability.", "OK");
            return false;
        }
        return true;
    }

    private void AddCurrentSelectionToOrder()
    {
        var toppings = AvailableToppings.Where(t => t.IsSelected).Select(t => t.Topping).ToList();
        var orderLine = new AddOrderLineDto
        {
            MenuItemId = MenuItemId,
            Quantity = Quantity,
            Size = SelectedSize,
            Toppings = toppings
        };
        _currentOrderItems.Add(orderLine);
    }

    private Task DisplayAlert(string title, string message, string cancel)
    {
        if (Application.Current?.MainPage != null)
            return Application.Current.MainPage.DisplayAlert(title, message, cancel);
        return Task.CompletedTask;
    }
}

public partial class ToppingSelectionItem : ObservableObject
{
    public Topping Topping { get; set; }
    [ObservableProperty] private bool _isSelected;
}
