namespace RaceDay.MVC.Models;

public class OrganiserEnrolmentViewModel
{
    public int EnrolmentID { get; set; }

    public int ParticipantID { get; set; }

    public string ParticipantName { get; set; } = string.Empty;

    public string CategoryName { get; set; } = string.Empty;

    public DateTime EnrolmentDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public bool HasResult { get; set; }
}