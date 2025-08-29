using System.Collections.ObjectModel;
using System.Diagnostics;
using BubbleTeaShop.Backend.DTOs.OrderDtos;
using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTeaShop.Backend.Services;
using BubbleTesShop.Backend.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BubbleTeaShop.Client.ViewModels;

[QueryProperty(nameof(OrderId), "OrderId")]
public partial class OrderDetailsViewModel : ObservableObject
{
    private readonly IOrderService _orderService;
    
    [ObservableProperty]
    private OrderDto _currentOrder;

    [ObservableProperty]
    private int _orderId;

    [ObservableProperty]
    private ObservableCollection<OrderLineDto> _orderLinesDisplay;

    public OrderDetailsViewModel(IOrderService orderService)
    {
        _orderService = orderService;
        _orderLinesDisplay = new ObservableCollection<OrderLineDto>();
    }

    partial void OnOrderIdChanged(int value)
    {
        _ = LoadOrderAsync(value);
    }

    private async Task LoadOrderAsync(int orderId)
    {
        try
        {
            CurrentOrder = await _orderService.GetOrderByIdAsync(orderId);
            OrderLinesDisplay.Clear();
            foreach (var line in CurrentOrder.OrderLines)
            {
                OrderLinesDisplay.Add(line);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ERROR loading order {orderId}: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task Ok()
    {
        await Shell.Current.GoToAsync("..");
        
    }

    [RelayCommand]
    private async Task ChangeStatusAccepted()
    {
        if (CurrentOrder != null)
        {
            await _orderService.ChangeToAcceptedAsync(CurrentOrder.Id);
            CurrentOrder.Status = "ACCEPTED";
            OnPropertyChanged(nameof(CurrentOrder));
        }
    }

    [RelayCommand]
    private async Task ChangeStatusInProgress()
    {
        if (CurrentOrder != null)
        {
            await _orderService.ChangeToInProgressAsync(CurrentOrder.Id);
            CurrentOrder.Status = "IN_PROGRESS";
            OnPropertyChanged(nameof(CurrentOrder));
        }
    }

    [RelayCommand]
    private async Task ChangeStatusCompleted()
    {
        if (CurrentOrder != null)
        {
            await _orderService.ChangeToCompletedAsync(CurrentOrder.Id);
            CurrentOrder.Status = "COMPLETED";
            OnPropertyChanged(nameof(CurrentOrder));
        }
    }

    [RelayCommand]
    private async Task ChangeStatusCancelled()
    {
        if (CurrentOrder != null)
        {
            await _orderService.ChangeToCancelledAsync(CurrentOrder.Id);
            CurrentOrder.Status = "CANCELLED";
            OnPropertyChanged(nameof(CurrentOrder));
        }
    }
}