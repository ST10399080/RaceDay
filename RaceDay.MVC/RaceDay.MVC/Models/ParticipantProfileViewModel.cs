using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;

public class ParticipantProfileViewModel
{
    public int UserID { get; set; }

    [Required]
    [MaxLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Display(Name = "Surname")]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string? ProfilePictureURL { get; set; }
}