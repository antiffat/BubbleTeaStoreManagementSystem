using System.ComponentModel.DataAnnotations;
using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class OrderLine
{
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }

    [Required]
    public int Quantity { get; set; }

    // New navigation property for the many-to-many relationship with Topping
    public ICollection<OrderLineToppingMapping> OrderLineToppings { get; set; }

    [Required]
    public Size Size { get; set; }

    // Navigation properties to represent the relationships
    public Order Order { get; set; }
    public MenuItem MenuItem { get; set; }
}