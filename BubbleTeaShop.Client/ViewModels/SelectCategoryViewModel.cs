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
        await Shell.Current.GoToAsync($"SelectMenuItemPage?Category={category}");
    }

    private async void ExecuteCancel()
    {
        await Shell.Current.GoToAsync("//MainPage"); 
    }
}