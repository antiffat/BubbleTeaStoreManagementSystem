using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Core.Enums;

namespace BubbleTeaShop.Core.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(20)] // Max length to accommodate phone number
    public string Phone { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MaxLength(250)]
    public string Address { get; set; }

    [Required]
    public float Salary { get; set; }

    // Optional properties based on role
    public int? HygieneScore { get; set; }
    public int? RegisterProficiency { get; set; }
    public int? TrainingSessionsConducted { get; set; }

    // Navigation properties for relationships
    public ICollection<AssignmentHistory> AssignmentHistories { get; set; }
    public ICollection<EmployeeCertificate> EmployeeCertificates { get; set; }
    public ICollection<EmployeeRoleMapping> EmployeeRoles { get; set; }
    
    // Navigation property for the worksIn association (many-to-many)
    public ICollection<Shift> WorksInShifts { get; set; }
    
    // Navigation property for the manages association (one-to-many, reverse navigation)
    public ICollection<Shift> ManagedShifts { get; set; }
}