using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1.Models;

public class Department
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(100)]
    [Display(Name = "Department manager")]
    public string ManagerName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(25)]
    [Phone]
    public string? Phone { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Office location")]
    public string OfficeLocation { get; set; } = string.Empty;
}
