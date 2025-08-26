using System.ComponentModel.DataAnnotations.Schema;
using BubbleTesShop.Backend.Enums;
using Microsoft.EntityFrameworkCore;

namespace BubbleTesShop.Backend.Models;

public class EmployeeRoleMapping
{
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }

    public EmployeeRole Role { get; set; }
}