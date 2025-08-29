using BubbleTeaShop.Client.ViewModels;
using BubbleTeaShop.Backend.Enums;
using BubbleTesShop.Backend.Enums;
using Size = BubbleTeaShop.Backend.Enums.Size;

namespace BubbleTeaShop.Client.Views;

public partial class CustomizeOrderLinePage : ContentPage
{
    private readonly CustomizeOrderLineViewModel _vm;

    public CustomizeOrderLinePage(CustomizeOrderLineViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }
    private void OnToppingCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        if (checkBox == null) return;

        if (checkBox.BindingContext is not Topping topping) return;
        
        if (BindingContext is not CustomizeOrderLineViewModel viewModel) return;

        if (e.Value)
        {
            if (!viewModel.SelectedToppings.Contains(topping))
            {
                viewModel.SelectedToppings.Add(topping);
            }
        }
        else
        {
            if (viewModel.SelectedToppings.Contains(topping))
            {
                viewModel.SelectedToppings.Remove(topping);
            }
        }
    }
    
    private void OnSizeCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value) return; 
        if (sender is not RadioButton rb) return;
        if (BindingContext is not CustomizeOrderLineViewModel vm) return;
        if (rb.BindingContext is Size size)
            vm.SelectedSize = size;
    }
}