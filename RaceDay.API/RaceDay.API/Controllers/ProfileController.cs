using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models.DTOs;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly RaceDayDbContext _context;

    public ProfileController(RaceDayDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Returns the profile of the currently authenticated user.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
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

        var user = await _context.Users
            .Where(u => u.UserID == userId)
            .Select(u => new
            {
                u.UserID,
                u.FirstName,
                u.Surname,
                u.Email,
                u.Role,
                u.ProfilePictureURL
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound(new
            {
                message = "User profile not found."
            });
        }

        return Ok(user);
    }

    /// <summary>
    /// Updates the authenticated user's own profile.
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateMyProfile(
        [FromBody] ProfileUpdateRequest request)
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

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.UserID == userId);

        if (user == null)
        {
            return NotFound(new
            {
                message = "User profile not found."
            });
        }

        var firstName = request.FirstName.Trim();
        var surname = request.Surname.Trim();
        var email = request.Email.Trim();

        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(surname) ||
            string.IsNullOrWhiteSpace(email))
        {
            return BadRequest(new
            {
                message = "First name, surname, and email are required."
            });
        }

        var emailExists = await _context.Users
            .AnyAsync(u =>
                u.Email == email &&
                u.UserID != userId);

        if (emailExists)
        {
            return Conflict(new
            {
                message = "This email address is already in use."
            });
        }

        user.FirstName = firstName;
        user.Surname = surname;
        user.Email = email;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Profile updated successfully.",
            user.UserID,
            user.FirstName,
            user.Surname,
            user.Email,
            user.Role,
            user.ProfilePictureURL
        });
    }
}
