using System.ComponentModel.DataAnnotations;

namespace SSD_Lab1.Models;

public class BoardMessage
{
    public int Id { get; set; }

    [Required, StringLength(1000)]
    [Display(Name = "Message")]
    public string Content { get; set; } = string.Empty;

    [StringLength(200)]
    [Display(Name = "Posted by")]
    public string AuthorName { get; set; } = string.Empty;

    public string AuthorId { get; set; } = string.Empty;

    [Display(Name = "Posted at (UTC)")]
    public DateTime CreatedAtUtc { get; set; }

    public ApplicationUser? Author { get; set; }
}
