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

    // ---- uncomment this later and delete the other
    // protected override void OnAppearing()
    // {
    //     base.OnAppearing();
    //     Debug.WriteLine("SelectMenuItemPage appeared");
    // }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("SelectMenuItemPage OnAppearing called");
    
        // Check if the category parameter was received
        if (_viewModel != null)
        {
            Debug.WriteLine($"Current category: {_viewModel.Category}");
        }
    }
}