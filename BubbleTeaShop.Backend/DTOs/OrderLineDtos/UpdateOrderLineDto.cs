using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.OrderLineDtos;

public class UpdateOrderLineDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Size Size { get; set; }
    public IEnumerable<Topping> Toppings { get; set; }
}