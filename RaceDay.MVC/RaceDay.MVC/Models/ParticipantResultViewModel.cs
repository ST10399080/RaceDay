namespace RaceDay.MVC.Models;

public class ParticipantResultViewModel
{
    public int ResultID { get; set; }
    public int EnrolmentID { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public TimeSpan FinishTime { get; set; }
    public int Position { get; set; }
}