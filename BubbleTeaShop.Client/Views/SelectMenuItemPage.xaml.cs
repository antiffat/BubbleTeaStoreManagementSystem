using System.Diagnostics;
using BubbleTeaShop.Client.ViewModels;

namespace BubbleTeaShop.Client.Views;

public partial class SelectMenuItemPage : ContentPage
{
    private readonly SelectMenuItemViewModel _viewModel;

    public SelectMenuItemPage(SelectMenuItemViewModel viewModel)
    {
        Debug.WriteLine("SelectMenuItemPage constructor called");
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("SelectMenuItemPage OnAppearing called");
    
        if (_viewModel != null)
        {
            Debug.WriteLine($"Current category: {_viewModel.Category}");
        }
    }
}