namespace RaceDay.MVC.Models;

public class OrganiserEventViewModel
{
    public int EventID { get; set; }

    public string EventName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    public string Location { get; set; } = string.Empty;

    public decimal Distance { get; set; }

    public string EventType { get; set; } = string.Empty;

    public int OrganiserID { get; set; }
}
