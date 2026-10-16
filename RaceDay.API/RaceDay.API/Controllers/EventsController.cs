using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EventsController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all RaceDay events.
    /// Organisers and Participants can view events.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Organiser,Participant")]
    public async Task<IActionResult> GetEvents()
    {
        var events = await _context.Events
            .AsNoTracking()
            .Select(e => new
            {
                e.EventID,
                e.EventName,
                e.Description,
                e.EventDate,
                e.Location,
                e.Distance,
                e.EventType,
                e.OrganiserID
            })
            .ToListAsync();

        return Ok(events);
    }

    /// <summary>
    /// Creates a new RaceDay event.
    /// Only Organisers can create events.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateEvent(
        [FromBody] RaceDay.API.Models.Event newEvent)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var organiserExists = await _context.Users
            .AnyAsync(u =>
                u.UserID == newEvent.OrganiserID &&
                u.Role == "Organiser");

        if (!organiserExists)
        {
            return BadRequest(new
            {
                message = "The specified Organiser does not exist."
            });
        }

        newEvent.EventID = 0;

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEvents),
            new { id = newEvent.EventID },
            newEvent);
    }
}
