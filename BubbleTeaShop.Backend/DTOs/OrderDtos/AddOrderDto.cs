using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTesShop.Backend.DTOs;

namespace BubbleTeaShop.Backend.DTOs.OrderDtos;

public class AddOrderDto
{
    public DateTime OrderDateTime { get; set; } = DateTime.Now;
    public OrderStatus Status { get; set; } = OrderStatus.PENDING;
    public IEnumerable<AddOrderLineDto> OrderLines { get; set; }
}