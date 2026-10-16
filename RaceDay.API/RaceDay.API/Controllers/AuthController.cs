using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    private readonly IConfiguration _configuration;

    public AuthController(
        RaceDayDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
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

    /// <summary>
    /// Authenticates a user and returns a JWT access token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "JWT signing key is missing.");

        var jwtIssuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException(
                "JWT issuer is missing.");

        var jwtAudience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException(
                "JWT audience is missing.");

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserID.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.Surname}"),
            new(ClaimTypes.Role, user.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Ok(new
        {
            message = "Login successful.",
            accessToken,
            tokenType = "Bearer",
            expiresIn = 3600,
            user = new
            {
                user.UserID,
                user.FirstName,
                user.Surname,
                user.Email,
                user.Role
            }
        });
    }

    /// <summary>
    /// DEVELOPMENT ONLY: Creates an Organiser account.
    /// Remove this endpoint before deployment.
    /// </summary>
    [HttpPost("create-organiser-dev")]
    public async Task<IActionResult> CreateOrganiserDev(
        [FromBody] RegisterRequest request)
    {
        if (!HttpContext.RequestServices
            .GetRequiredService<IWebHostEnvironment>()
            .IsDevelopment())
        {
            return NotFound();
        }

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
            Role = "Organiser"
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Development Organiser created successfully.",
            user.UserID,
            user.FirstName,
            user.Surname,
            user.Email,
            user.Role
        });
    }
}
