using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class EmployeeRoleMapping
{
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public EmployeeRole Role { get; set; }
}