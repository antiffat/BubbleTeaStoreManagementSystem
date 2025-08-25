using System.ComponentModel.DataAnnotations;

namespace BubbleTeaShop.Core.Models;

public class AssignmentHistory
{
    // Composite key for the association with an attribute
    public int StoreId { get; set; }
    public int EmployeeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    // Navigation properties to represent the relationships
    public Store Store { get; set; }
    public Employee Employee { get; set; }
}