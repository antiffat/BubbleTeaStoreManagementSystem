using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.Models;

public class OrderLine
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }

    [Required]
    public int Quantity { get; set; }

    public ICollection<OrderLineToppingMapping> OrderLineToppings { get; set; }

    [Required]
    public Size Size { get; set; }

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