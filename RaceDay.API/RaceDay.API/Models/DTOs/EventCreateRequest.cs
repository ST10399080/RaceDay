using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models.DTOs;

public class EventCreateRequest
{
    [Required]
    [MaxLength(100)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(0.1, 1000)]
    public decimal Distance { get; set; }

    [Required]
    [MaxLength(20)]
    public string EventType { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? BannerImageURL { get; set; }
}
