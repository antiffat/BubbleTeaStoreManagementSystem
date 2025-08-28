using BubbleTeaShop.Client.ViewModels;

namespace BubbleTeaShop.Client;

public partial class MainPage : ContentPage
{
	private readonly OrderHistoryViewModel _vm;

	public MainPage(OrderHistoryViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		BindingContext = _vm;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _vm.InitializeAsync();
	}
}

