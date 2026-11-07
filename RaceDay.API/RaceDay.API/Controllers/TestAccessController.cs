using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RaceDay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestAccessController : ControllerBase
{
    /// <summary>
    /// Tests access for authenticated users.
    /// </summary>
    [HttpGet("authenticated")]
    [Authorize]
    public IActionResult Authenticated()
    {
        return Ok(new
        {
            message = "You are authenticated.",
            userId = User.FindFirst(
                System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value,
            role = User.FindFirst(
                System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }

    /// <summary>
    /// Tests access for Organisers only.
    /// </summary>
    [HttpGet("organiser")]
    [Authorize(Roles = "Organiser")]
    public IActionResult OrganiserOnly()
    {
        return Ok(new
        {
            message = "Organiser access granted."
        });
    }

    /// <summary>
    /// Tests access for Participants only.
    /// </summary>
    [HttpGet("participant")]
    [Authorize(Roles = "Participant")]
    public IActionResult ParticipantOnly()
    {
        return Ok(new
        {
            message = "Participant access granted."
        });
    }
}
