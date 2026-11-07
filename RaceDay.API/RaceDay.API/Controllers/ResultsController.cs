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
public class ResultsController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ResultsController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Records a participant's finish time and position.
    /// Only the Organiser of the event may record results.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> CreateResult(
        [FromBody] ResultCreateRequest request)
    {
        var userIdClaim = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var organiserId))
        {
            return Unauthorized(new
            {
                message = "The authenticated Organizer ID could not be determined."
            });
        }

        var enrolment = await _context.Enrolments
            .Include(e => e.Event)
            .Include(e => e.Result)
            .FirstOrDefaultAsync(e =>
                e.EnrolmentID == request.EnrolmentID);

        if (enrolment == null)
        {
            return NotFound(new
            {
                message = "Enrolment not found."
            });
        }

        if (enrolment.Event.OrganiserID != organiserId)
        {
            return Forbid();
        }

        if (enrolment.Result != null)
        {
            return Conflict(new
            {
                message = "A result has already been recorded for this enrolment."
            });
        }

        if (request.FinishTime <= TimeSpan.Zero)
        {
            return BadRequest(new
            {
                message = "Finish time must be greater than zero."
            });
        }

        if (request.Position <= 0)
        {
            return BadRequest(new
            {
                message = "Finishing position must be greater than zero."
            });
        }

        var result = new Result
        {
            EnrolmentID = enrolment.EnrolmentID,
            FinishTime = request.FinishTime,
            Position = request.Position
        };

        _context.Results.Add(result);
        await _context.SaveChangesAsync();

        return Created(
            $"/api/Results/{result.ResultID}",
            new
            {
                result.ResultID,
                result.EnrolmentID,
                result.FinishTime,
                result.Position
            });
    }

    /// <summary>
    /// Returns results belonging to the authenticated Participant.
    /// </summary>
    [HttpGet("my-results")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> GetMyResults()
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

        var results = await _context.Results
            .Where(r => r.Enrolment.ParticipantID == participant.ParticipantID)
            .Select(r => new
            {
                r.ResultID,
                r.EnrolmentID,
                EventName = r.Enrolment.Event.EventName,
                EventDate = r.Enrolment.Event.EventDate,
                EventType = r.Enrolment.Event.EventType,
                CategoryName = r.Enrolment.Category.CategoryName,
                r.FinishTime,
                r.Position
            })
            .ToListAsync();

        return Ok(results);
    }
}
