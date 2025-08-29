using System.Collections.ObjectModel;
using BubbleTesShop.Backend.DTOs;
using BubbleTesShop.Backend.DTOs.MenuItemDtos;
using BubbleTesShop.Backend.DTOs.OrderDtos;
using BubbleTesShop.Backend.Enums;
using BubbleTesShop.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Size = BubbleTeaShop.Backend.Enums.Size;

namespace BubbleTeaShop.Client.ViewModels;

public partial class OrderDetailsSummaryViewModel : ObservableObject
{
    private readonly IOrderService _orderService;
        private readonly IMenuItemService _menuItemService;

        [ObservableProperty]
        private ObservableCollection<OrderLineDto> _orderSummaryItems = new();

        [ObservableProperty]
        private double _orderTotalPrice;

        public IRelayCommand CancelCommand { get; }
        public IRelayCommand PlaceFinalOrderCommand { get; }

        public OrderDetailsSummaryViewModel(IOrderService orderService, IMenuItemService menuItemService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _menuItemService = menuItemService ?? throw new ArgumentNullException(nameof(menuItemService));

            CancelCommand = new RelayCommand(ExecuteCancel);
            PlaceFinalOrderCommand = new RelayCommand(async () => await ExecutePlaceFinalOrderAsync());
        }

        public async Task InitializeAsync()
        {
            await LoadOrderSummaryItemsFromStaticListAsync();
        }

        private async Task LoadOrderSummaryItemsFromStaticListAsync()
        {
            OrderSummaryItems.Clear();
            OrderTotalPrice = 0;

            var current = CustomizeOrderLineViewModel.GetCurrentOrderItems();
            if (current == null || !current.Any())
                return;

            double total = 0;

            foreach (var addLine in current)
            {
                // Try fetch menu item (for name/basePrice); fall back to placeholders if not present
                MenuItemDto menuItem = null;
                try
                {
                    menuItem = await _menuItemService.GetMenuItemByIdAsync(addLine.MenuItemId);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"MenuItem lookup failed for id {addLine.MenuItemId}: {ex.Message}");
                }

                var basePrice = menuItem?.BasePrice ?? 0.0;
                var name = menuItem?.Name ?? $"Item #{addLine.MenuItemId}";

                var itemTotal = ComputeItemTotalPrice(basePrice, addLine.Quantity, addLine.Size, addLine.Toppings);

                // Build an OrderLineDto for UI binding. DTOs are simple POCOs in your solution and are safe to use here.
                var uiLine = new OrderLineDto
                {
                    // DTO fields used by UI - fill as needed
                    MenuItemId = addLine.MenuItemId,
                    MenuItemName = name,
                    BasePrice = basePrice,
                    Quantity = addLine.Quantity,
                    Size = addLine.Size,
                    Toppings = addLine.Toppings?.ToList() ?? new System.Collections.Generic.List<Topping>(),
                    ItemTotalPrice = itemTotal
                };

                OrderSummaryItems.Add(uiLine);
                total += itemTotal;
            }

            OrderTotalPrice = total;
        }

        private double ComputeItemTotalPrice(double basePrice, int quantity, Size size, System.Collections.Generic.IEnumerable<Topping> toppings)
        {
            double total = basePrice * quantity;

            if (size == Size.M) total += 4.0 * quantity;
            else if (size == Size.L) total += 5.0 * quantity;

            if (toppings != null)
                total += (toppings.Count()) * 4.0 * quantity;

            return total;
        }

        private async void ExecuteCancel()
        { 
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async Task ExecutePlaceFinalOrderAsync()
        {
            if (!OrderSummaryItems.Any())
            {
                await DisplayAlertAsync("No Items", "There are no items to finalize in this order.", "OK");
                return;
            }

            try
            {
                // Build AddOrderDto from the static CustomizeOrderLineViewModel list
                var current = CustomizeOrderLineViewModel.GetCurrentOrderItems();
                if (current == null || !current.Any())
                {
                    await DisplayAlertAsync("No Items", "There are no items to finalize in this order.", "OK");
                    return;
                }

                var addOrder = new AddOrderDto
                {
                    OrderDateTime = DateTime.Now,
                    Status = OrderStatus.PENDING,
                    OrderLines = current.Select(c => new AddOrderLineDto
                    {
                        MenuItemId = c.MenuItemId,
                        Quantity = c.Quantity,
                        Size = c.Size,
                        Toppings = c.Toppings?.ToList() ?? new System.Collections.Generic.List<Topping>()
                    }).ToList()
                };

                var createdId = await _orderService.CreateOrderAsync(addOrder);

                CustomizeOrderLineViewModel.ClearCurrentOrderItems();

                await DisplayAlertAsync("Order Placed!", $"Your order has been placed!\nOrder Id: {createdId}\nTotal: {OrderTotalPrice:C}", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR placing order: {ex}");
                await DisplayAlertAsync("Order Error", $"Failed to place order: {ex.Message}", "OK");
            }
        }

        private Task DisplayAlertAsync(string title, string message, string cancel)
        {
            if (Application.Current?.MainPage != null)
                return Application.Current.MainPage.DisplayAlert(title, message, cancel);
            return Task.CompletedTask;
        }
    
}