using System;
using System.ComponentModel.DataAnnotations;
using BubbleTesShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class AssignmentHistory
{
    public int StoreId { get; set; }
    public int EmployeeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public Store Store { get; set; }
    public Employee Employee { get; set; }
}