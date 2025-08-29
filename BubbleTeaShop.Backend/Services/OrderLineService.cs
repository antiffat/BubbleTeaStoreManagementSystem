using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;
using BubbleTeaShop.Backend.Repositories;
using BubbleTeaShop.Backend.Repositories.MenuItem;
using BubbleTesShop.Backend.DTOs;

namespace BubbleTeaShop.Backend.Services;

public class OrderLineService : IOrderLineService
{
    private readonly IOrderLineRepository _orderLineRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;

    public OrderLineService(
        IOrderLineRepository orderLineRepository,
        IOrderRepository orderRepository,
        IMenuItemRepository menuItemRepository)
    {
        _orderLineRepository = orderLineRepository;
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<IEnumerable<OrderLineDto>> GetAllOrderLinesAsync()
    {
        var list = await _orderLineRepository.GetAllOrderLinesAsync();
        return list.Select(MapToDto);
    }

    public async Task<OrderLineDto> GetOrderLineByIdAsync(int id)
    {
        if (!await _orderLineRepository.OrderLineExistsAsync(id))
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");

        var ol = await _orderLineRepository.GetOrderLineByIdAsync(id);
        return MapToDto(ol);
    }

    public async Task<IEnumerable<OrderLineDto>> GetOrderLinesByMenuItemIdAsync(int menuItemId)
    {
        if (!await _menuItemRepository.MenuItemExistsAsync(menuItemId))
            throw new KeyNotFoundException("MenuItem with given ID does not exist.");

        var list = await _orderLineRepository.GetOrderLineByMenuItemIdAsync(menuItemId);
        return list.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderLineDto>> GetOrderLinesByOrderIdAsync(int orderId)
    {
        if (!await _orderRepository.OrderExistsAsync(orderId))
            throw new KeyNotFoundException("Order with given ID does not exist.");

        var list = await _orderLineRepository.GetOrderLineByOrderIdAsync(orderId);
        return list.Select(MapToDto);
    }

    public async Task<int> AddOrderLineAsync(AddOrderLineDto dto)
    {
        if (!await _orderRepository.OrderExistsAsync(dto.OrderId))
            throw new KeyNotFoundException("Order with given ID does not exist.");
        if (!await _menuItemRepository.MenuItemExistsAsync(dto.MenuItemId))
            throw new KeyNotFoundException("MenuItem with given ID does not exist.");

        if (dto.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(dto.Quantity));

        var orderLine = new OrderLine
        {
            OrderId = dto.OrderId,
            MenuItemId = dto.MenuItemId,
            Quantity = dto.Quantity,
            Size = dto.Size,
            OrderLineToppings = dto.Toppings?.Select(t => new OrderLineToppingMapping { Topping = t }).ToList()
        };

        await _orderLineRepository.AddOrderLineAsync(orderLine);

        return orderLine.Id;
    }

    public async Task UpdateOrderLineAsync(UpdateOrderLineDto dto)
    {
        if (!await _orderLineRepository.OrderLineExistsAsync(dto.Id))
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");

        var existing = await _orderLineRepository.GetOrderLineByIdAsync(dto.Id);
        if (existing == null)
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");

        if (dto.Quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(dto.Quantity));

        existing.Quantity = dto.Quantity;
        existing.Size = dto.Size;

        existing.OrderLineToppings = dto.Toppings?
            .Select(t => new OrderLineToppingMapping { OrderLineId = existing.Id, Topping = t })
            .ToList() ?? new List<OrderLineToppingMapping>();

        await _orderLineRepository.UpdateOrderLineAsync(existing);
    }

    public async Task DeleteOrderLineAsync(int id)
    {
        var deleted = await _orderLineRepository.TryDeleteOrderLineIfOrderHasMoreThanOneAsync(id);

        if (!deleted)
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");
    }
    
    public async Task<double> GetItemTotalPriceAsync(int orderLineId)
    {
        if (!await _orderLineRepository.OrderLineExistsAsync(orderLineId))
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");

        var orderLine = await _orderLineRepository.GetOrderLineByIdAsync(orderLineId);
        if (orderLine == null)
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");
        
        return orderLine.ItemTotalPrice;
    }
    
    public async Task UpdateQuantityAsync(int orderLineId, int newQuantity)
    {
        if (newQuantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));

        if (!await _orderLineRepository.OrderLineExistsAsync(orderLineId))
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");

        var existing = await _orderLineRepository.GetOrderLineByIdAsync(orderLineId);
        if (existing == null)
            throw new KeyNotFoundException("OrderLine with given ID does not exist.");

        existing.Quantity = newQuantity;

        await _orderLineRepository.UpdateOrderLineAsync(existing);
    }

    public double CalculateItemTotalPrice(OrderLine ol)
    {
        if (ol != null && ol.MenuItem != null)
            return ol.ItemTotalPrice;

        // just in case
        double basePrice = ol.MenuItem?.BasePrice ?? 0.0;
        double total = basePrice * (ol?.Quantity ?? 0);

        if (ol?.Size == Size.M)
            total += 4.0 * (ol?.Quantity ?? 0);
        else if (ol?.Size == Size.L)
            total += 5.0 * (ol?.Quantity ?? 0);

        if (ol?.OrderLineToppings != null)
            total += ol.OrderLineToppings.Count * 4.0 * (ol.Quantity);

        return total;
    }
    
    private OrderLineDto MapToDto(OrderLine ol)
    {
        var dto = new OrderLineDto
        {
            Id = ol.Id,
            OrderId = ol.OrderId,
            MenuItemId = ol.MenuItemId,
            MenuItemName = ol.MenuItem?.Name,
            BasePrice = ol.MenuItem?.BasePrice ?? 0.0,
            Quantity = ol.Quantity,
            Size = ol.Size,
            Toppings = ol.OrderLineToppings?.Select(t => t.Topping).ToList() ?? new List<Topping>(),
            ItemTotalPrice = CalculateItemTotalPrice(ol)
        };
        return dto;
    }
}