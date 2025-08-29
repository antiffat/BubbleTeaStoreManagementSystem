using System.Collections.ObjectModel;
using System.Diagnostics;
using BubbleTesShop.Backend.DTOs;
using BubbleTesShop.Backend.Enums;
using BubbleTesShop.Backend.Services;
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
    
    [ObservableProperty]
    private int _menuItemId;
    
    [ObservableProperty]
    private string _menuItemName;
    
    [ObservableProperty]
    private double _basePrice;
    
    [ObservableProperty]
    private int _quantity = 1;
    
    [ObservableProperty]
    private Size _selectedSize = Size.S;
    
    [ObservableProperty]
    private ObservableCollection<Topping> _selectedToppings = new();

    public ObservableCollection<Topping> AvailableToppings { get; } = new()
    {
        Topping.TAPIOCA,
        Topping.KONJAC
    };

    public ObservableCollection<Size> AvailableSizes { get; } = new()
    {
        Size.S,
        Size.M,
        Size.L
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

    public static IReadOnlyList<AddOrderLineDto> GetCurrentOrderItems()
    {
        return _currentOrderItems.AsReadOnly();
    }

    public static void ClearCurrentOrderItems()
    {
        _currentOrderItems.Clear();
    }

    private async void ExecuteCancel()
    {
        ClearCurrentOrderItems();
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void ExecuteAddMoreItems()
    {
        Debug.WriteLine($"AddMoreItems command executed. Current Quantity: {Quantity}");

        if (MenuItemId <= 0)
        {
            await DisplayAlert("Error", "No menu item selected.", "OK");
            return;
        }

        if (Quantity <= 0)
        {
            await DisplayAlert("Invalid Quantity", "Please enter a quantity greater than 0.", "OK");
            return;
        }

        // Check stock availability
        try
        {
            bool isAvailable = await _menuItemService.IsAvailableAsync(MenuItemId, Quantity);
            if (!isAvailable)
            {
                var menuItem = await _menuItemService.GetMenuItemByIdAsync(MenuItemId);
                await DisplayAlert("Out of Stock", 
                    $"Only {menuItem.StockQuantity} of {MenuItemName} are available. Cannot add {Quantity}.", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking availability: {ex.Message}");
            await DisplayAlert("Error", "Could not check item availability.", "OK");
            return;
        }

        // Add to current order items
        var orderLine = new AddOrderLineDto
        {
            MenuItemId = MenuItemId,
            Quantity = Quantity,
            Size = SelectedSize,
            Toppings = SelectedToppings.ToList()
        };
        
        _currentOrderItems.Add(orderLine);
        
        await DisplayAlert("Item Added", 
            $"{Quantity}x {MenuItemName} added to your order! You can add more items or finalize.", "OK");
        
        await Shell.Current.GoToAsync("SelectCategoryPage"); 
    }
    
    private async void ExecuteFinalizeOrder()
    {
        if (MenuItemId <= 0)
        {
            await DisplayAlert("Error", "No menu item selected.", "OK");
            return;
        }

        if (Quantity <= 0)
        {
            await DisplayAlert("Invalid Quantity", "Please enter a quantity greater than 0.", "OK");
            return;
        }

        // Check stock availability
        try
        {
            bool isAvailable = await _menuItemService.IsAvailableAsync(MenuItemId, Quantity);
            if (!isAvailable)
            {
                var menuItem = await _menuItemService.GetMenuItemByIdAsync(MenuItemId);
                await DisplayAlert("Out of Stock", 
                    $"Only {menuItem.StockQuantity} of {MenuItemName} are available. Cannot finalize with {Quantity}.", "OK");
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error checking availability: {ex.Message}");
            await DisplayAlert("Error", "Could not check item availability.", "OK");
            return;
        }

        // Add current item to order
        var orderLine = new AddOrderLineDto
        {
            MenuItemId = MenuItemId,
            Quantity = Quantity,
            Size = SelectedSize,
            Toppings = SelectedToppings.ToList()
        };
        
        _currentOrderItems.Add(orderLine);

        if (_currentOrderItems.Count == 0)
        {
            await DisplayAlert("No Items", "Your order has no items. Please add at least one item.", "OK");
            return;
        }

        await Shell.Current.GoToAsync("ReviewOrderPage");
    }

    private async Task DisplayAlert(string title, string message, string cancel)
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}