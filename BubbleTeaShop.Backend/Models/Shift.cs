using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Backend.Models;
using DayOfWeek = BubbleTesShop.Backend.Enums.DayOfWeek;

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

    public ICollection<Employee> Employees { get; set; }

    [ForeignKey("ManagingEmployee")]
    public int? ManagingEmployeeId { get; set; } 
    public Employee? ManagingEmployee { get; set; }
}