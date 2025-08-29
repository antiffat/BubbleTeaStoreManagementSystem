using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTeaShop.Backend.Enums;
using BubbleTesShop.Backend.DTOs;

namespace BubbleTeaShop.Backend.DTOs.OrderDtos;

public class UpdateOrderDto
{
    public int Id { get; set; }
    public DateTime? OrderDateTime { get; set; }
    public OrderStatus? Status { get; set; }
    public IEnumerable<AddOrderLineDto> OrderLines { get; set; }
}