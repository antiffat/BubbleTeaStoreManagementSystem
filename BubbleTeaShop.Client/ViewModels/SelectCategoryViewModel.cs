using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BubbleTeaShop.Client.ViewModels;

public partial class SelectCategoryViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<string> _categories;

    public IRelayCommand<string> SelectCategoryCommand { get; }
    public IRelayCommand CancelCommand { get; }

    public SelectCategoryViewModel()
    {
        Categories = new ObservableCollection<string>
        {
            "FruitTea",
            "MilkTea",
            "Frappe"
        };

        SelectCategoryCommand = new RelayCommand<string>(ExecuteSelectCategory);
        CancelCommand = new RelayCommand(ExecuteCancel);
    }

    private async void ExecuteSelectCategory(string category)
    {
        Debug.WriteLine($"Attempting to navigate to SelectMenuItemPage with category: {category}");
        try
        {
            await Shell.Current.GoToAsync($"SelectMenuItemPage?Category={category}");            
            Debug.WriteLine("Navigation attempted successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Navigation failed: {ex.Message}");
            await DisplayAlert("Error", $"Cannot navigate: {ex.Message}", "OK");
        }
    }
    private async Task DisplayAlert(string title, string message, string cancel)
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }

    private async void ExecuteCancel()
    {
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}"); 
    }
}