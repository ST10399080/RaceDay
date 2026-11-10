using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models.DTOs;

public class EnrolmentStatusUpdateRequest
{
    [Required]
    [RegularExpression(
        "^(Approved|Rejected)$",
        ErrorMessage = "Status must be Approved or Rejected.")]
    public string Status { get; set; } = string.Empty;
}
