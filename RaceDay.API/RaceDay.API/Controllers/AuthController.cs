
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Models.DTOs;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RaceDayDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(RaceDayDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    /// <summary>
    /// Registers a new participant account.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email);

        if (emailExists)
        {
            return Conflict(new
            {
                message = "An account with this email already exists."
            });
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            Surname = request.Surname.Trim(),
            Email = email,
            Role = "Participant"
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password);

        var participant = new Participant
        {
            User = user
        };

        _context.Users.Add(user);
        _context.Participants.Add(participant);

        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Participant registered successfully.",
            user.UserID,
            user.FirstName,
            user.Surname,
            user.Email,
            user.Role
        });
    }
}
