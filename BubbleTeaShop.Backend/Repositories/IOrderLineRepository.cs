using BubbleTeaShop.Backend.Models;

namespace BubbleTesShop.Backend.Repositories;

public interface IOrderLineRepository
{
    Task<IEnumerable<OrderLine>> GetAllOrderLinesAsync();
    Task<OrderLine> GetOrderLineByIdAsync(int id);
    Task AddOrderLineAsync(OrderLine orderLine);
    Task UpdateOrderLineAsync(OrderLine orderLine);
    Task DeleteOrderLineAsync(int id);
    Task<bool> OrderLineExistsAsync(int id);
    Task<IEnumerable<OrderLine>> GetOrderLineByMenuItemIdAsync(int menuItemId);
    Task<IEnumerable<OrderLine>> GetOrderLineByOrderIdAsync(int orderId);
    Task<bool> OrderLineExistsMenuItemOrderAsync(int menuItemId, int orderId);

    Task<OrderLine> GetOrderLineWithOrderAndOrderLinesAsync(int id);

    Task<bool> TryDeleteOrderLineIfOrderHasMoreThanOneAsync(int orderLineId);
}