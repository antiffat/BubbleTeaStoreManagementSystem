using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BubbleTesShop.Backend.Models;

public class Shift
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public DayOfWeek DayOfWeek { get; set; }

    // Navigation property for the worksIn association (many-to-many)
    public ICollection<Employee> Employees { get; set; }

    // Foreign key for the manages association (one-to-many)
    [ForeignKey("ManagingEmployee")]
    public int? ManagingEmployeeId { get; set; } 
    public Employee? ManagingEmployee { get; set; }
}