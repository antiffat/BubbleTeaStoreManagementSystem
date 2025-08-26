using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTeaShop.Backend.Models;

public class Store
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [MaxLength(250)]
    public string Location { get; set; }
    
    // no data annotations since it is not possible to store const or static attributes in the database
    // const is implicitly static, since I do not intend to change the value of it runtime, const is the best choice.
    public const string Currency = "PLN";

    public ICollection<MenuItem> MenuItems { get; set; }
    
    public ICollection<AssignmentHistory> AssignmentHistories { get; set; }
}