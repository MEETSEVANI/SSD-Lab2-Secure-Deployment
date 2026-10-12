using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SSD_Lab1.Models;

/// <summary>
/// Extends IdentityUser with the profile fields that are not already supplied by Identity.
/// UserName, Email, and PhoneNumber are inherited from IdentityUser.
/// </summary>
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(50)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }
}
