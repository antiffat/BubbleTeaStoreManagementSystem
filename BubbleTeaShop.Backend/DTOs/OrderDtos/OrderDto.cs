using BubbleTeaShop.Backend.DTOs.OrderLineDtos;
using BubbleTesShop.Backend.DTOs;

namespace BubbleTeaShop.Backend.DTOs.OrderDtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDateTime { get; set; }
    public string Status { get; set; } 
    public IEnumerable<OrderLineDto> OrderLines { get; set; }
    public double OrderTotalPrice { get; set; }
}