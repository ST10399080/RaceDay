using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication;

namespace RaceDay.MVC.Controllers;

[Authorize(Roles = "Participant")]
public class ParticipantController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ParticipantController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public IActionResult Dashboard()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Events()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync("api/Events");

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] =
                "Could not retrieve sporting events from the RaceDay API.";

            return View(new List<RaceDay.MVC.Models.OrganiserEventViewModel>());
        }

        var events = await response.Content.ReadFromJsonAsync<
            List<RaceDay.MVC.Models.OrganiserEventViewModel>>()
            ?? new List<RaceDay.MVC.Models.OrganiserEventViewModel>();

        events = events
            .OrderBy(e => e.EventDate)
            .ToList();

        return View(events);
    }
}
