using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RaceDay.MVC.Models;

public class CreateEventViewModel
{
    [Required]
    [MaxLength(100)]
    [Display(Name = "Event Name")]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Event Date")]
    public DateTime EventDate { get; set; } = DateTime.Today.AddDays(1);

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(0.1, 1000)]
    [Display(Name = "Distance (km)")]
    public decimal Distance { get; set; }

    [Required]
    [MaxLength(20)]
    [Display(Name = "Event Type")]
    public string EventType { get; set; } = string.Empty;

    [Display(Name = "Event Banner")]
    public IFormFile? BannerImage { get; set; }
}
