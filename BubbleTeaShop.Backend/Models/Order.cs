using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BubbleTesShop.Backend.Enums;
using BubbleTesShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime OrderDateTime { get; set; }

    [Required]
    public OrderStatus Status { get; set; }

    public ICollection<OrderLine> OrderLines { get; set; }
    
    [NotMapped]
    public double OrderTotalPrice
    {
        get
        {
            return OrderLines?.Sum(ol => ol.ItemTotalPrice) ?? 0;
        }
    }
}