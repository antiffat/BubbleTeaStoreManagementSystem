using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.DTOs;

namespace BubbleTesShop.Backend.Services;

public interface IOrderLineService
{
    Task<IEnumerable<OrderLineDto>> GetAllOrderLinesAsync();
    Task<OrderLineDto> GetOrderLineByIdAsync(int id);
    Task<int> AddOrderLineAsync(AddOrderLineDto orderLineDto);
    Task UpdateOrderLineAsync(UpdateOrderLineDto orderLineDto);
    Task DeleteOrderLineAsync(int id);
    Task<double> GetItemTotalPriceAsync(int orderLineId);
    Task UpdateQuantityAsync(int orderLineId, int newQuantity);
    Task<IEnumerable<OrderLineDto>> GetOrderLinesByMenuItemIdAsync(int menuItemId);
    Task<IEnumerable<OrderLineDto>> GetOrderLinesByOrderIdAsync(int orderId);
    
}