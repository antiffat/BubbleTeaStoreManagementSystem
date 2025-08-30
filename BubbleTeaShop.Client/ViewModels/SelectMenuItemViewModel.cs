using System.Collections.ObjectModel;
using System.Diagnostics;
using BubbleTeaShop.Backend.DTOs.MenuItemDtos;
using BubbleTeaShop.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BubbleTeaShop.Client.ViewModels;

[QueryProperty(nameof(Category), "Category")]
public partial class SelectMenuItemViewModel : ObservableObject
{
    private readonly IMenuItemService _menuItemService;
    
    [ObservableProperty]
    private string _category; 

    [ObservableProperty]
    private ObservableCollection<MenuItemDto> _menuItems = new();

    [ObservableProperty]
    private bool _isLoading;

    public IRelayCommand<MenuItemDto> SelectMenuItemCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public SelectMenuItemViewModel(IMenuItemService menuItemService)
    {
        Debug.WriteLine("SelectMenuItemViewModel constructor called");
        _menuItemService = menuItemService;
        
        SelectMenuItemCommand = new RelayCommand<MenuItemDto>(ExecuteSelectMenuItem);
        CancelCommand = new RelayCommand(ExecuteCancel);
    }

    partial void OnCategoryChanged(string value)
    {
        Debug.WriteLine($"Category changed to: {value}");
        LoadMenuItemsByCategory(value);
    }

    private async void LoadMenuItemsByCategory(string category)
    {
        if (string.IsNullOrEmpty(category))
            return;

        IsLoading = true;
        
        try
        {
            var allItems = await _menuItemService.GetAllMenuItemsAsync();
            var filteredItems = allItems.Where(item => 
                string.Equals(item.ItemType, category, StringComparison.OrdinalIgnoreCase));
            
            MenuItems.Clear();
            foreach (var item in filteredItems)
            {
                MenuItems.Add(item);
            }
            
            Debug.WriteLine($"Loaded {MenuItems.Count} items for category: {category}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading menu items: {ex.Message}");
            await DisplayAlert("Error", $"Failed to load menu items: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void ExecuteSelectMenuItem(MenuItemDto selectedItem)
    {
        if (selectedItem == null) 
        {
            Debug.WriteLine("No menu item selected");
            return;
        }

        Debug.WriteLine($"Selected menu item: {selectedItem.Name}");

        try
        {
            bool isAvailable = await _menuItemService.IsAvailableAsync(selectedItem.Id);
            if (!isAvailable)
            {
                await DisplayAlert("Item Not Available", 
                    $"{selectedItem.Name} is currently out of stock.", "OK");
                return;
            }

            await Shell.Current.GoToAsync(
                $"CustomizeOrderLinePage?MenuItemId={selectedItem.Id}" +
                $"&MenuItemName={Uri.EscapeDataString(selectedItem.Name)}" +
                $"&BasePrice={selectedItem.BasePrice}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error selecting menu item: {ex.Message}");
            await DisplayAlert("Error", $"Failed to select menu item: {ex.Message}", "OK");
        }
    }

    private async void ExecuteCancel()
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async Task DisplayAlert(string title, string message, string cancel)
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}