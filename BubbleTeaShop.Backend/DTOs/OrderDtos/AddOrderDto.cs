namespace BubbleTesShop.Backend.DTOs.OrderDtos;

public class AddOrderDto
{
    public DateTime OrderDateTime { get; set; } = DateTime.Now;
    public BubbleTesShop.Backend.Enums.OrderStatus Status { get; set; } = BubbleTesShop.Backend.Enums.OrderStatus.PENDING;
    public IEnumerable<AddOrderLineDto> OrderLines { get; set; }
}