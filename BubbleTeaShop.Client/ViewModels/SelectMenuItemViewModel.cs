using System.Collections.ObjectModel;
using System.Diagnostics;
using BubbleTesShop.Backend.DTOs.MenuItemDtos;
using BubbleTesShop.Backend.Services;
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
    private ObservableCollection<MenuItemDto> _menuItems;

    public IRelayCommand<MenuItemDto> SelectMenuItemCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public SelectMenuItemViewModel(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
        
        SelectMenuItemCommand = new RelayCommand<MenuItemDto>(ExecuteSelectMenuItem);
        CancelCommand = new RelayCommand(ExecuteCancel);
    }

    partial void OnCategoryChanged(string value)
    {
        LoadMenuItems(value);
    }

    private async void LoadMenuItems(string category)
    {
        try
        {
            var allItems = await _menuItemService.GetAllMenuItemsAsync();
            MenuItems = new ObservableCollection<MenuItemDto>(
                allItems.Where(item => item.ItemType == category));
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading menu items: {ex.Message}");
        }
    }

    private async void ExecuteSelectMenuItem(MenuItemDto menuItem)
    {
        if (menuItem != null)
        {
            await Shell.Current.GoToAsync(
                $"ConfigureMenuItemPage?MenuItemId={menuItem.Id}&MenuItemName={menuItem.Name}&BasePrice={menuItem.BasePrice}");
        }
    }

    private async void ExecuteCancel()
    {
        await Shell.Current.GoToAsync("..");
    }
}