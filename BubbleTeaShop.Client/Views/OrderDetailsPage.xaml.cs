using BubbleTeaShop.Client.ViewModels;

namespace BubbleTeaShop.Client.Views;

public partial class OrderDetailsPage : ContentPage
{
    private readonly OrderDetailsViewModel _vm;
    public OrderDetailsPage(OrderDetailsViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }
    
}