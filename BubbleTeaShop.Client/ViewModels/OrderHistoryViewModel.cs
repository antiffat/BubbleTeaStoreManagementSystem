using System.Collections.ObjectModel;
using BubbleTeaShop.Backend.DTOs.OrderDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BubbleTeaShop.Client.ViewModels;

public partial class OrderHistoryViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;

        [ObservableProperty]
        private ObservableCollection<OrderDto> orders = new ObservableCollection<OrderDto>();

        public IAsyncRelayCommand<OrderDto> ChangeStatusCommand { get; }
        public IAsyncRelayCommand CreateOrderCommand { get; }
        public IAsyncRelayCommand<OrderDto> ViewDetailsCommand { get; }
        public IAsyncRelayCommand RefreshCommand { get; }

        public OrderHistoryViewModel(IOrderService orderService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

            ChangeStatusCommand = new AsyncRelayCommand<OrderDto>(ExecuteChangeStatusAsync);
            CreateOrderCommand = new AsyncRelayCommand(ExecuteCreateOrderAsync);
            ViewDetailsCommand = new AsyncRelayCommand<OrderDto>(ExecuteViewDetailsAsync);
            RefreshCommand = new AsyncRelayCommand(LoadOrdersAsync);
        }

        public async Task InitializeAsync()
        {
            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                Orders.Clear();
                var list = await _orderService.GetAllOrdersAsync();
                foreach (var o in list)
                    Orders.Add(o);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Failed to load orders: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        private async Task ExecuteCreateOrderAsync()
        {
            // CustomizeOrderLineViewModel.ClearCurrentOrderItems();
            await Shell.Current.GoToAsync("SelectCategoryPage");
        }

        private async Task ExecuteViewDetailsAsync(OrderDto selectedOrder)
        {
            Console.WriteLine($"View Details clicked for order: {selectedOrder?.Id}");
    
            if (selectedOrder == null)
            {
                await Shell.Current.DisplayAlert("Error", "No order selected for details.", "OK");
                return;
            }

            try
            {
                await Shell.Current.GoToAsync($"OrderDetailsPage?OrderId={selectedOrder.Id}");
                Console.WriteLine("Navigation attempted");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Navigation error: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", $"Cannot navigate: {ex.Message}", "OK");
            }
        }

        private async Task ExecuteChangeStatusAsync(OrderDto orderDto)
        {
            if (orderDto == null) return;

            try
            {
                switch (orderDto.Status)
                {
                    case nameof(OrderStatus.PENDING):
                        await _orderService.ChangeToAcceptedAsync(orderDto.Id);
                        orderDto.Status = nameof(OrderStatus.ACCEPTED);
                        break;

                    case nameof(OrderStatus.ACCEPTED):
                        await _orderService.ChangeToInProgressAsync(orderDto.Id);
                        orderDto.Status = nameof(OrderStatus.IN_PROGRESS);
                        break;

                    case nameof(OrderStatus.IN_PROGRESS):
                        await _orderService.ChangeToReadyToPickupAsync(orderDto.Id);
                        orderDto.Status = nameof(OrderStatus.READY_TO_PICKUP);
                        break;

                    case nameof(OrderStatus.READY_TO_PICKUP):
                        await _orderService.ChangeToCompletedAsync(orderDto.Id);
                        orderDto.Status = nameof(OrderStatus.COMPLETED);
                        break;

                    default:
                        await DisplayAlertAsync("Status Change", $"Order is already {orderDto.Status}. No further changes allowed.", "OK");
                        return;
                }

                var index = Orders.IndexOf(orderDto);
                if (index >= 0) Orders[index] = orderDto;

                await DisplayAlertAsync("Status Changed", $"Order status updated to: {orderDto.Status}", "OK");
            }
            catch (InvalidOperationException ex)
            {
                await DisplayAlertAsync("Status Change Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Unable to change status: {ex.Message}", "OK");
            }
        }

        private Task DisplayAlertAsync(string title, string message, string cancel)
        {
            if (Application.Current?.MainPage != null)
                return Application.Current.MainPage.DisplayAlert(title, message, cancel);
            return Task.CompletedTask;
        }
}