using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Backend.Enums;
using BubbleTeaShop.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BubbleTeaShop.Backend.Models;

public class EmployeeRoleMapping
{
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public EmployeeRole Role { get; set; }
}