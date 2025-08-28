namespace BubbleTesShop.Backend.DTOs.ShiftDtos;
using DayOfWeek = BubbleTesShop.Backend.Enums.DayOfWeek;


public class UpdateShiftDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public int? ManagingEmployeeId { get; set; }
    public List<int> EmployeeIds { get; set; } = null;
}