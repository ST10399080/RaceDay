using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class User
{
    public int UserID { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Surname { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ProfilePictureURL { get; set; }

    public Participant? Participant { get; set; }

    public ICollection<Event> OrganisedEvents { get; set; }
        = new List<Event>();
}
