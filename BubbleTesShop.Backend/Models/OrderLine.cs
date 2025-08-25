using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTesShop.Backend.Enums;

namespace BubbleTesShop.Backend.Models;

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
    
    [NotMapped]
    public double ItemTotalPrice
    {
        get
        {
            double total = MenuItem.BasePrice * Quantity;

            if (Size == Enums.Size.M)
            {
                total += 4.0 * Quantity;
            }
            else if (Size == Enums.Size.L)
            {
                total += 5.0 * Quantity;
            }
            
            if (OrderLineToppings != null)
            {
                total += OrderLineToppings.Count * 4.0 * Quantity;
            }
        
            return total;
        }
    }
}