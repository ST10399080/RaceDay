using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models;

public class Category
{
    public int CategoryID { get; set; }

    [Required]
    public int EventID { get; set; }

    public Event Event { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    public ICollection<Enrolment> Enrolments { get; set; }
        = new List<Enrolment>();
}
