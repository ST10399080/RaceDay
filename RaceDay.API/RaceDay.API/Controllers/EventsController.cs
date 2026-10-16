using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using System.Security.Claims;
using RaceDay.API.Models;
using RaceDay.API.Models.DTOs;

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
        [FromBody] EventCreateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var organiserId))
        {
            return Unauthorized(new
            {
                message = "The authenticated Organiser ID could not be determined."
            });
        }

        var organiserExists = await _context.Users
            .AnyAsync(u =>
                u.UserID == organiserId &&
                u.Role == "Organiser");

        if (!organiserExists)
        {
            return Forbid();
        }

        var newEvent = new RaceDay.API.Models.Event
        {
            OrganiserID = organiserId,
            EventName = request.EventName.Trim(),
            Description = request.Description.Trim(),
            EventDate = request.EventDate,
            Location = request.Location.Trim(),
            Distance = request.Distance,
            EventType = request.EventType.Trim(),
            BannerImageURL = request.BannerImageURL
        };

        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEvents),
            new { id = newEvent.EventID },
            new
            {
                newEvent.EventID,
                newEvent.OrganiserID,
                newEvent.EventName,
                newEvent.Description,
                newEvent.EventDate,
                newEvent.Location,
                newEvent.Distance,
                newEvent.EventType,
                newEvent.BannerImageURL
            });
    }

    /// <summary>
    /// Updates an existing event.
    /// Only the Organiser who owns the event can update it.
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> UpdateEvent(
        int id,
        [FromBody] EventCreateRequest request)
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

        var existingEvent = await _context.Events
            .FirstOrDefaultAsync(e => e.EventID == id);

        if (existingEvent == null)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        if (existingEvent.OrganiserID != organiserId)
        {
            return Forbid();
        }

        existingEvent.EventName = request.EventName.Trim();
        existingEvent.Description = request.Description.Trim();
        existingEvent.EventDate = request.EventDate;
        existingEvent.Location = request.Location.Trim();
        existingEvent.Distance = request.Distance;
        existingEvent.EventType = request.EventType.Trim();
        existingEvent.BannerImageURL = request.BannerImageURL;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Event updated successfully.",
            existingEvent.EventID,
            existingEvent.OrganiserID,
            existingEvent.EventName,
            existingEvent.Description,
            existingEvent.EventDate,
            existingEvent.Location,
            existingEvent.Distance,
            existingEvent.EventType,
            existingEvent.BannerImageURL
        });
    }
}
