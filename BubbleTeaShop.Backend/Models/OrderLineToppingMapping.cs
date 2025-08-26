using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.Enums;

namespace BubbleTeaShop.Backend.Models;

// join table. Topping is an enum, which can be handled as a value type in EF Core. We can directly reference it in the OrderLineTopping join table
public class OrderLineToppingMapping
{
    public int OrderLineId { get; set; }
    public Topping Topping { get; set; }

    public OrderLine OrderLine { get; set; }
}