using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTesShop.Backend.Models;

public abstract class MenuItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    public double BasePrice { get; set; }

    [Required]
    public int StockQuantity { get; set; }

    // New navigation property for the many-to-many relationship with Allergen
    public ICollection<MenuItemAllergen> MenuItemAllergens { get; set; } 

    // Navigation property for the relationship with OrderLine
    public ICollection<OrderLine> OrderLines { get; set; }
    
    // Navigation property for the many-to-many relationship with Store
    public ICollection<Store> Stores { get; set; }
}