using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Result
{
    public int ResultID { get; set; }

    [Required]
    public int EnrolmentID { get; set; }

    public Enrolment Enrolment { get; set; } = null!;

    public TimeSpan FinishTime { get; set; }

    public int Position { get; set; }
}
