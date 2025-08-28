using BubbleTesShop.Backend.Enums;

namespace BubbleTesShop.Backend.DTOs.EmployeeDtos;

public class UpdateEmployeeDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public float Salary { get; set; }
    public int? HygieneScore { get; set; }
    public int? RegisterProficiency { get; set; }
    public int? TrainingSessionsConducted { get; set; }
    public List<EmployeeRole> Roles { get; set; } = new();
    public List<string> Certificates { get; set; } = new();
}