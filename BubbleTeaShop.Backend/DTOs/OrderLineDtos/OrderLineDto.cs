using BubbleTeaShop.Backend.Enums;
using BubbleTesShop.Backend.Enums;

namespace BubbleTesShop.Backend.DTOs;

public class OrderLineDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public string? MenuItemName { get; set; }
    public int Quantity { get; set; }
    public Size Size { get; set; }
    public IEnumerable<Topping> Toppings { get; set; }
    public double ItemTotalPrice { get; set; }
}