using BubbleTeaShop.Client.ViewModels;

namespace BubbleTeaShop.Client.Views;

public partial class OrderDetailsSummaryPage : ContentPage
{
    private readonly OrderDetailsSummaryViewModel _vm;

    public OrderDetailsSummaryPage(OrderDetailsSummaryViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            System.Diagnostics.Debug.WriteLine("OrderDetailsSummaryPage.OnAppearing -> calling VM.InitializeAsync()");
            await _vm.InitializeAsync();
            System.Diagnostics.Debug.WriteLine($"OrderDetailsSummaryPage.OnAppearing -> loaded {_vm.OrderSummaryItems?.Count ?? 0} items, total {_vm.OrderTotalPrice:C}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing OrderDetailsSummaryPage VM: {ex}");
            // show friendly error to user
            if (Application.Current?.MainPage != null)
                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load order summary: {ex.Message}", "OK");
        }
    }
}