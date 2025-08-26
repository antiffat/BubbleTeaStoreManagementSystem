using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTeaShop.Backend.Models;

public class MenuItemAllergen
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    // Navigation property for the many-to-many relationship with MenuItem
    public ICollection<MenuItem> MenuItems { get; set; }
}