using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.OrderLineDtos;

public class AddOrderLineDto
{
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
    public Size Size { get; set; }
    public IEnumerable<Topping> Toppings { get; set; }
}