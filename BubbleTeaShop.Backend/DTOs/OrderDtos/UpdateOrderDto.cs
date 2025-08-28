namespace BubbleTesShop.Backend.DTOs.OrderDtos;

public class UpdateOrderDto
{
    public int Id { get; set; }
    public DateTime? OrderDateTime { get; set; }
    public BubbleTesShop.Backend.Enums.OrderStatus? Status { get; set; }
    public IEnumerable<AddOrderLineDto> OrderLines { get; set; }
}