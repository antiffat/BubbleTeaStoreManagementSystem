using DayOfWeek = BubbleTesShop.Backend.Enums.DayOfWeek;

namespace BubbleTesShop.Backend.DTOs.ShiftDtos;

public class ShiftDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public List<int> EmployeeIds { get; set; } = new();
    public int? ManagingEmployeeId { get; set; }
    public string ManagingEmployeeName { get; set; }
}