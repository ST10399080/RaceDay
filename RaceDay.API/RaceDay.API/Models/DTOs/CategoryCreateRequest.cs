using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.Models.DTOs;

public class CategoryCreateRequest
{
    [Required]
    [MaxLength(50)]
    public string CategoryName { get; set; } = string.Empty;
}
