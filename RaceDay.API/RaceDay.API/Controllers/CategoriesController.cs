using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Models.DTOs;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public CategoriesController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves categories for an event.
    /// Organisers and Participants can view categories.
    /// </summary>
    [HttpGet("event/{eventId:int}")]
    [Authorize(Roles = "Organiser,Participant")]
    public async Task<IActionResult> GetCategories(int eventId)
    {
        var eventExists = await _context.Events
            .AnyAsync(e => e.EventID == eventId);

        if (!eventExists)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        var categories = await _context.Categories
            .AsNoTracking()
            .Where(c => c.EventID == eventId)
            .Select(c => new
            {
                c.CategoryID,
                c.EventID,
                c.CategoryName
            })
            .ToListAsync();

        return Ok(categories);
    }


    /// <summary>
    /// Creates a category for an event owned by the Organiser.
    /// </summary>
    [HttpPost("event/{eventId:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateCategory(
        int eventId,
        [FromBody] CategoryCreateRequest request)
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var organiserId))
        {
            return Unauthorized(new
            {
                message = "The authenticated Organiser ID could not be determined."
            });
        }

        var eventItem = await _context.Events
            .FirstOrDefaultAsync(e => e.EventID == eventId);

        if (eventItem == null)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        if (eventItem.OrganiserID != organiserId)
        {
            return Forbid();
        }

        var categoryName = request.CategoryName.Trim();

        if (string.IsNullOrWhiteSpace(categoryName))
        {
            return BadRequest(new
            {
                message = "Category name is required."
            });
        }

        var categoryExists = await _context.Categories
            .AnyAsync(c =>
                c.EventID == eventId &&
                c.CategoryName == categoryName);

        if (categoryExists)
        {
            return Conflict(new
            {
                message = "This category already exists for the event."
            });
        }

        var category = new Category
        {
            EventID = eventId,
            CategoryName = categoryName
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new
        {
            category.CategoryID,
            category.EventID,
            category.CategoryName
        });
    }
}
