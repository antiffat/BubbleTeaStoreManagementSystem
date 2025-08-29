using BubbleTeaShop.Backend.DTOs.AssignmentHistoryDtos;
using BubbleTeaShop.Backend.Enums;

namespace BubbleTeaShop.Backend.DTOs.EmployeeDtos;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; } // 9 digits
    public string Email { get; set; }
    public string Address { get; set; }
    public float Salary { get; set; }

    // nullable role-specific values (returned as-is)
    public int? HygieneScore { get; set; }
    public int? RegisterProficiency { get; set; }
    public int? TrainingSessionsConducted { get; set; }

    // roles and certificates
    public List<EmployeeRole> Roles { get; set; } = new();
    public List<string> Certificates { get; set; } = new();

    public List<AssignmentHistoryDto> AssignmentHistories { get; set; } = new();

}