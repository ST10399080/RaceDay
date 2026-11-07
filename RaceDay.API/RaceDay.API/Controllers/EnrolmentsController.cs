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
public class EnrolmentsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public EnrolmentsController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Enrols the authenticated Participant in an event and category.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> CreateEnrolment(
        [FromBody] EnrolmentCreateRequest request)
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new
            {
                message = "The authenticated user ID could not be determined."
            });
        }

        var participant = await _context.Participants
            .FirstOrDefaultAsync(p => p.UserID == userId);

        if (participant == null)
        {
            return NotFound(new
            {
                message = "Participant profile not found."
            });
        }

        var eventExists = await _context.Events
            .AnyAsync(e => e.EventID == request.EventID);

        if (!eventExists)
        {
            return NotFound(new
            {
                message = "Event not found."
            });
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(c =>
                c.CategoryID == request.CategoryID);

        if (category == null)
        {
            return NotFound(new
            {
                message = "Category not found."
            });
        }

        if (category.EventID != request.EventID)
        {
            return BadRequest(new
            {
                message = "The selected category does not belong to this event."
            });
        }

        var alreadyEnrolled = await _context.Enrolments
            .AnyAsync(e =>
                e.ParticipantID == participant.ParticipantID &&
                e.EventID == request.EventID);

        if (alreadyEnrolled)
        {
            return Conflict(new
            {
                message = "You are already enrolled in this event."
            });
        }

        var enrolment = new Enrolment
        {
            ParticipantID = participant.ParticipantID,
            EventID = request.EventID,
            CategoryID = request.CategoryID,
            EnrolmentDate = DateTime.UtcNow,
            Status = "Pending"
        };

        _context.Enrolments.Add(enrolment);
        await _context.SaveChangesAsync();

        return Created(
            $"/api/Enrolments/{enrolment.EnrolmentID}",
            new
            {
                enrolment.EnrolmentID,
                enrolment.ParticipantID,
                enrolment.EventID,
                enrolment.CategoryID,
                enrolment.EnrolmentDate,
                enrolment.Status
            });
    }

    /// <summary>
    /// Returns enrolments for an event owned by the authenticated Organiser.
    /// </summary>
    [HttpGet("event/{eventId:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> GetEventEnrolments(int eventId)
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

        var enrolments = await _context.Enrolments
            .Where(e => e.EventID == eventId)
            .Select(e => new
            {
                e.EnrolmentID,
                e.ParticipantID,
                ParticipantName = e.Participant.User.FirstName
                    + " " + e.Participant.User.Surname,
                e.EventID,
                e.CategoryID,
                CategoryName = e.Category.CategoryName,
                e.EnrolmentDate,
                e.Status
            })
            .ToListAsync();

        return Ok(enrolments);
    }
}
