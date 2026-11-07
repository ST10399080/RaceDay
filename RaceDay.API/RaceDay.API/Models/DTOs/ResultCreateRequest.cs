using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models.DTOs;

public class ResultCreateRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int EnrolmentID { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Position { get; set; }
}
