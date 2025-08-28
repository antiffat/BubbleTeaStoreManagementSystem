using BubbleTeaShop.Client.ViewModels;

namespace BubbleTeaShop.Client.Views;

public partial class SelectCategoryPage : ContentPage
{
    private readonly SelectCategoryViewModel _vm;

    public SelectCategoryPage(SelectCategoryViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }
}