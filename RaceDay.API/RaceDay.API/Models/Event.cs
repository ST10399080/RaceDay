using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Event
{
    public int EventID { get; set; }

    [Required]
    public int OrganiserID { get; set; }

    public User Organiser { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    public string EventName { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    [Range(0.1, 1000)]
    public decimal Distance { get; set; }

    [Required]
    [MaxLength(20)]
    public string EventType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? BannerImageURL { get; set; }

    public ICollection<Category> Categories { get; set; }
        = new List<Category>();

    public ICollection<Enrolment> Enrolments { get; set; }
        = new List<Enrolment>();
}
