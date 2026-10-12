using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1.Models;

public class Company
{
    [Display(Name = "Company ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, 200, ErrorMessage = "Years in business must be between 0 and 200.")]
    [Display(Name = "Years in business")]
    public int YearsInBusiness { get; set; }

    [Required]
    [StringLength(2048)]
    [Url(ErrorMessage = "Enter a valid website URL, including https://.")]
    public string Website { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Province { get; set; }
}
