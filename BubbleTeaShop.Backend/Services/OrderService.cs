using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.DTOs;
using BubbleTesShop.Backend.DTOs.OrderDtos;
using BubbleTesShop.Backend.Enums;
using BubbleTesShop.Backend.Repositories;

namespace BubbleTesShop.Backend.Services;

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
        
        await _orderRepository.DeleteOrderAsync(id);
    }

    private OrderDto MapToDto(Order order)
    {
        if (order == null) return null;

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
                Quantity = ol.Quantity,
                Size = ol.Size,
                Toppings = ol.OrderLineToppings?.Select(t => t.Topping).ToList() ?? new List<Topping>(),
                ItemTotalPrice = ol.ItemTotalPrice
            }).ToList() ?? new List<OrderLineDto>(),
            OrderTotalPrice = order.OrderTotalPrice
        };
    }
}