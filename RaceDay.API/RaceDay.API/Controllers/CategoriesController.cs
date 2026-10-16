using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

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
}
