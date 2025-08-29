using System.Diagnostics;
using BubbleTeaShop.Backend.DTOs.OrderDtos;
using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;
using BubbleTeaShop.Backend.Repositories;
using BubbleTeaShop.Backend.Repositories.MenuItem;
using BubbleTesShop.Backend.DTOs;

namespace BubbleTeaShop.Backend.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderLineRepository _orderLineRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IOrderLineRepository orderLineRepository,
        IMenuItemRepository menuItemRepository)
    {
        _orderRepository = orderRepository;
        _orderLineRepository = orderLineRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        return orders.Select(MapToDto);
    }

    public async Task<OrderDto> GetOrderByIdAsync(int id)
    {
        if (!await _orderRepository.OrderExistsAsync(id))
            throw new KeyNotFoundException("Order with given ID does not exist.");

        var order = await _orderRepository.GetOrderByIdAsync(id);
        return MapToDto(order);
    }

    public async Task<int> CreateOrderAsync(AddOrderDto dto)
    {
        var addLines = dto.OrderLines?.ToList() ?? new List<AddOrderLineDto>();
        if (!addLines.Any())
            throw new ArgumentException("Order must contain at least one OrderLine.", nameof(dto.OrderLines));

        if (addLines.Any(l => l.Quantity <= 0))
            throw new ArgumentException("OrderLine quantity must be greater than zero.");

        var menuIds = addLines.Select(l => l.MenuItemId).Distinct().ToList();
        var existingMenuItems = await _menuItemRepository.GetMenuItemsByIdsAsync(menuIds);
        if (existingMenuItems.Count != menuIds.Count)
            throw new KeyNotFoundException("One or more MenuItems do not exist.");

        var order = new Order
        {
            OrderDateTime = dto.OrderDateTime,
            Status = dto.Status,
            OrderLines = addLines.Select(l => new OrderLine
            {
                MenuItemId = l.MenuItemId,
                Quantity = l.Quantity,
                Size = l.Size,
                OrderLineToppings = l.Toppings?.Select(t => new OrderLineToppingMapping { Topping = t }).ToList()
            }).ToList()
        };

        await _orderRepository.AddOrderAsync(order);
        return order.Id;
    }

    public async Task UpdateOrderAsync(UpdateOrderDto dto)
    {
        if (!await _orderRepository.OrderExistsAsync(dto.Id))
            throw new KeyNotFoundException("Order with given ID does not exist.");

        var order = await _orderRepository.GetOrderByIdAsync(dto.Id);
        if (order == null)
            throw new KeyNotFoundException("Order with given ID does not exist.");

        if (dto.OrderDateTime.HasValue)
            order.OrderDateTime = dto.OrderDateTime.Value;
        if (dto.Status.HasValue)
            order.Status = dto.Status.Value;

        if (dto.OrderLines != null)
        {
            var newLines = dto.OrderLines.ToList();
            if (!newLines.Any())
                throw new ArgumentException("Order must contain at least one OrderLine.", nameof(dto.OrderLines));

            foreach (var l in newLines)
            {
                if (l.Quantity <= 0)
                    throw new ArgumentException("Each OrderLine quantity must be greater than zero.", nameof(l.Quantity));
            }

            order.OrderLines = newLines.Select(l => new OrderLine
            {
                MenuItemId = l.MenuItemId,
                Quantity = l.Quantity,
                Size = l.Size,
                OrderLineToppings = l.Toppings?.Select(t => new OrderLineToppingMapping { Topping = t }).ToList()
            }).ToList();
        }

        await _orderRepository.UpdateOrderAsync(order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        if (!await _orderRepository.OrderExistsAsync(id))
            throw new KeyNotFoundException("Order with given ID does not exist.");
        
        var ols = await _orderLineRepository.GetOrderLineByOrderIdAsync(id);
        if (ols.Any())
            throw new InvalidOperationException("Cannot delete employee who still has assignment histories.");


        await _orderRepository.DeleteOrderAsync(id);
    }
    
    // STATEEEEEE
    private void EnsureNotFinal(Order order)
    {
        if (order.Status == OrderStatus.COMPLETED || order.Status == OrderStatus.CANCELLED)
            throw new InvalidOperationException("Cannot change status of an order that is Completed or Cancelled.");
    }

    private async Task<Order> LoadExistingOrderOrThrow(int orderId)
    {
        if (!await _orderRepository.OrderExistsAsync(orderId))
            throw new KeyNotFoundException("Order with given ID does not exist.");

        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new KeyNotFoundException("Order with given ID does not exist.");

        return order;
    }
    
    private void EnsureCorrectTransition(Order order, OrderStatus expectedFromStatus, OrderStatus toStatus)
    {
    if (order.Status != expectedFromStatus)
    {
        throw new InvalidOperationException($"Cannot change status from {order.Status} to {toStatus}. The order must be in the {expectedFromStatus} state.");
    }
}

public async Task ChangeToAcceptedAsync(int orderId)
{
    var order = await LoadExistingOrderOrThrow(orderId);
    EnsureCorrectTransition(order, OrderStatus.PENDING, OrderStatus.ACCEPTED);
    order.Status = OrderStatus.ACCEPTED;
    await _orderRepository.UpdateOrderAsync(order);
}

public async Task ChangeToInProgressAsync(int orderId)
{
    var order = await LoadExistingOrderOrThrow(orderId);
    EnsureCorrectTransition(order, OrderStatus.ACCEPTED, OrderStatus.IN_PROGRESS);
    order.Status = OrderStatus.IN_PROGRESS;
    await _orderRepository.UpdateOrderAsync(order);
}

public async Task ChangeToReadyToPickupAsync(int orderId)
{
    var order = await LoadExistingOrderOrThrow(orderId);
    EnsureCorrectTransition(order, OrderStatus.IN_PROGRESS, OrderStatus.READY_TO_PICKUP);
    order.Status = OrderStatus.READY_TO_PICKUP;
    await _orderRepository.UpdateOrderAsync(order);
}

public async Task ChangeToCompletedAsync(int orderId)
{
    var order = await LoadExistingOrderOrThrow(orderId);
    EnsureCorrectTransition(order, OrderStatus.READY_TO_PICKUP, OrderStatus.COMPLETED);
    order.Status = OrderStatus.COMPLETED;
    await _orderRepository.UpdateOrderAsync(order);
}

public async Task ChangeToCancelledAsync(int orderId)
{
    var order = await LoadExistingOrderOrThrow(orderId);
    
    // The diagram shows transitions from PENDING and ACCEPTED to CANCELLED.
    if (order.Status != OrderStatus.PENDING && order.Status != OrderStatus.ACCEPTED)
    {
        // An exception is thrown if the current state is not one of the allowed states.
        throw new InvalidOperationException($"Cannot cancel an order from the {order.Status} state. It must be in PENDING or ACCEPTED.");
    }
    
    order.Status = OrderStatus.CANCELLED;
    await _orderRepository.UpdateOrderAsync(order);
}


    private OrderDto MapToDto(Order order)
    {
        if (order == null) return null;
        Debug.WriteLine($"MapToDto: Order {order.Id} has {order.OrderLines?.Count ?? 0} lines");

        foreach (var ol in order.OrderLines ?? Enumerable.Empty<OrderLine>())
        {
            Debug.WriteLine($"  OL {ol.Id} toppings count (entity): {ol.OrderLineToppings?.Count ?? 0}");
            if (ol.OrderLineToppings != null)
            {
                foreach (var t in ol.OrderLineToppings)
                {
                    Debug.WriteLine($"    raw topping type: {t.Topping.GetType().FullName} value: {(int)(object)t.Topping} name: {t.Topping.ToString()}");
                }
            }
        }
        return new OrderDto
        {
            Id = order.Id,
            OrderDateTime = order.OrderDateTime,
            Status = order.Status.ToString(),
            OrderLines = order.OrderLines?.Select(ol => new OrderLineDto
            {
                Id = ol.Id,
                OrderId = ol.OrderId,
                MenuItemId = ol.MenuItemId,
                MenuItemName = ol.MenuItem?.Name,
                BasePrice = ol.MenuItem?.BasePrice ?? 0.0,
                Quantity = ol.Quantity,
                Size = ol.Size,
                Toppings = ol.OrderLineToppings?.Select(t => t.Topping).ToList() ?? new List<Topping>(),
                ItemTotalPrice = ol.ItemTotalPrice
            }).ToList() ?? new List<OrderLineDto>(),
            OrderTotalPrice = order.OrderTotalPrice
        };
    }
}