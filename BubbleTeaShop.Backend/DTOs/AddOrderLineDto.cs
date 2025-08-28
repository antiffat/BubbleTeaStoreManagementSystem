using BubbleTeaShop.Backend.Enums;
using BubbleTesShop.Backend.Enums;

namespace BubbleTesShop.Backend.DTOs;

public class AddOrderLineDto
{
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
    public Size Size { get; set; }
    public IEnumerable<Topping> Toppings { get; set; }
}