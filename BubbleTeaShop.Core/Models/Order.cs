using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime OrderDateTime { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    // Navigation property for the relationship with OrderLine
    public ICollection<OrderLine> OrderLines { get; set; }
}