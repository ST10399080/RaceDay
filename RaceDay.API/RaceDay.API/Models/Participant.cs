using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models;

public class Participant
{
    public int ParticipantID { get; set; }

    [Required]
    public int UserID { get; set; }

    [ForeignKey(nameof(UserID))]
    public User User { get; set; } = null!;

    public ICollection<Enrolment> Enrolments { get; set; }
        = new List<Enrolment>();
}
