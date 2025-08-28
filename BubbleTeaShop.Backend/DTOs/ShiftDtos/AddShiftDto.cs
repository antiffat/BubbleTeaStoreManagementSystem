using DayOfWeek = BubbleTesShop.Backend.Enums.DayOfWeek;

namespace BubbleTesShop.Backend.DTOs.ShiftDtos;

public class AddShiftDto
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int ManagingEmployeeId { get; set; }
    public List<int> EmployeeIds { get; set; } = new();
}