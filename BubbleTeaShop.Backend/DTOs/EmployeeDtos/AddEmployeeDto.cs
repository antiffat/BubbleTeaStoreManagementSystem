using BubbleTeaShop.Backend.DTOs.AssignmentHistoryDtos;
using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.EmployeeDtos;

public class AddEmployeeDto
{
    public string FullName { get; set; }
    public string Phone { get; set; } // must be 9 digits
    public string Email { get; set; }
    public string Address { get; set; }
    public float Salary { get; set; }
    public int? HygieneScore { get; set; }
    public int? RegisterProficiency { get; set; }
    public int? TrainingSessionsConducted { get; set; }
    public List<EmployeeRole> Roles { get; set; } = new();
    public List<string> Certificates { get; set; } = new();
    public List<AddAssignmentHistoryDto> AssignmentHistories { get; set; } = new();
}