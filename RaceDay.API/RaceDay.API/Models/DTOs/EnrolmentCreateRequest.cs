using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models.DTOs;

public class EnrolmentCreateRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int EventID { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryID { get; set; }
}
