using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.DTOs;
using BubbleTesShop.Backend.DTOs.OrderDtos;

namespace BubbleTesShop.Backend.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto> GetOrderByIdAsync(int id);
    Task<int> CreateOrderAsync(AddOrderDto dto);
    Task UpdateOrderAsync(UpdateOrderDto dto);
    Task DeleteOrderAsync(int id);
    Task ChangeToAcceptedAsync(int orderId);
    Task ChangeToInProgressAsync(int orderId);
    Task ChangeToReadyToPickupAsync(int orderId);
    Task ChangeToCompletedAsync(int orderId);
    Task ChangeToCancelledAsync(int orderId);
}