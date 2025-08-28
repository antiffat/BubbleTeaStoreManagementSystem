using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTesShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class AssignmentHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } 
    public int StoreId { get; set; }
    public int EmployeeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public Store Store { get; set; }
    public Employee Employee { get; set; }
}