using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.Enums;

namespace BubbleTeaShop.Backend.Models;

// join table. Topping is an enum, which can be handled as a value type in EF Core. We can directly reference it in the OrderLineTopping join table
public class OrderLineToppingMapping
{
    public int OrderLineId { get; set; }
    public Topping Topping { get; set; } // The enum value itself can be part of the key

    // Navigation properties for the many-to-many relationship
    public OrderLine OrderLine { get; set; }
}