using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;

public class RecordResultViewModel
{
    [Required]
    public int EnrolmentID { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Position must be greater than zero.")]
    public int Position { get; set; }

    [Required]
    public TimeSpan FinishTime { get; set; }
}