using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Enrolment
{
    public int EnrolmentID { get; set; }

    [Required]
    public int ParticipantID { get; set; }

    public Participant Participant { get; set; } = null!;

    [Required]
    public int EventID { get; set; }

    public Event Event { get; set; } = null!;

    [Required]
    public int CategoryID { get; set; }

    public Category Category { get; set; } = null!;

    public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    public Result? Result { get; set; }
}
