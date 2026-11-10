using System.ComponentModel.DataAnnotations;

namespace RaceDay.MVC.Models;
public class EnrolmentCreateViewModel
{
    [Required]
    public int EventID { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryID { get; set; }
}