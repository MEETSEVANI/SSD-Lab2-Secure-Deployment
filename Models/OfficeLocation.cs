using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1.Models;

public class OfficeLocation
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "Office name")]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(150)]
    [Display(Name = "Street address")]
    public string Address { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Province { get; set; } = string.Empty;

    [Required, StringLength(12)]
    [Display(Name = "Postal code")]
    public string PostalCode { get; set; } = string.Empty;

    [Required, Phone, StringLength(25)]
    public string Phone { get; set; } = string.Empty;
}
