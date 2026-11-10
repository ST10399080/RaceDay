using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers;

[Authorize(Roles = "Participant")]
public class ParticipantController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ParticipantController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Dashboard()
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
            .Where(e => e.EventDate.Date >= DateTime.Today)
            .OrderBy(e => e.EventDate)
            .ToList();

        return View(events);
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

    [HttpGet]
    public async Task<IActionResult> EventDetails(int id)
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
                "Could not retrieve event details.";

            return RedirectToAction(nameof(Events));
        }

        var events = await response.Content.ReadFromJsonAsync<
            List<RaceDay.MVC.Models.OrganiserEventViewModel>>()
            ?? new List<RaceDay.MVC.Models.OrganiserEventViewModel>();

        var selectedEvent = events.FirstOrDefault(e => e.EventID == id);

        if (selectedEvent == null)
        {
            TempData["ErrorMessage"] = "The selected event could not be found.";
            return RedirectToAction(nameof(Events));
        }

        return View(selectedEvent);
    }

    [HttpGet]
    public async Task<IActionResult> Enrol(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync(
            $"api/Categories/event/{id}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] =
                "Could not retrieve categories for this event.";

            return RedirectToAction(nameof(Events));
        }

        var categories = await response.Content.ReadFromJsonAsync<
            List<RaceDay.MVC.Models.CategoryViewModel>>()
            ?? new List<RaceDay.MVC.Models.CategoryViewModel>();

        ViewBag.EventID = id;

        return View(categories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEnrolment(
    EnrolmentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] =
                "Please select a valid event category.";

            return RedirectToAction(nameof(Enrol), new { id = model.EventID });
        }

        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.PostAsJsonAsync(
            "api/Enrolments",
            model);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();

            TempData["ErrorMessage"] =
                "Enrolment failed. " + errorMessage;

            return RedirectToAction(nameof(Enrol), new { id = model.EventID });
        }

        TempData["SuccessMessage"] =
            "Your enrolment was submitted successfully. Your status is Pending.";

        return RedirectToAction(nameof(EventDetails), new { id = model.EventID });
    }

    [HttpGet]
    public async Task<IActionResult> MyEnrolments()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync(
            "api/Enrolments/my-enrolments");

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] =
                "Could not retrieve your enrolments.";

            return View(new List<ParticipantEnrolmentViewModel>());
        }

        var enrolments = await response.Content.ReadFromJsonAsync<
            List<ParticipantEnrolmentViewModel>>()
            ?? new List<ParticipantEnrolmentViewModel>();

        return View(enrolments);
    }
}
