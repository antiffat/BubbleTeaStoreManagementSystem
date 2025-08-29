using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BubbleTeaShop.Backend.Models;
using BubbleTesShop.Backend.Models;

namespace BubbleTeaShop.Backend.Models;

public class Employee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; }

    [Required]
    [MaxLength(9)] 
    public string Phone { get; set; }

    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MaxLength(250)]
    public string Address { get; set; }

    [Required]
    public float Salary { get; set; }

    public int? HygieneScore { get; set; }
    public int? RegisterProficiency { get; set; }
    public int? TrainingSessionsConducted { get; set; }

    public ICollection<AssignmentHistory> AssignmentHistories { get; set; }
    public ICollection<EmployeeCertificate> EmployeeCertificates { get; set; }
    public ICollection<EmployeeRoleMapping> EmployeeRoles { get; set; }
    
    public ICollection<Shift> WorksInShifts { get; set; }
    
    public ICollection<Shift> ManagedShifts { get; set; }
}