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

            return View(new List<OrganiserEventViewModel>());
        }

        var events = await response.Content.ReadFromJsonAsync<
            List<OrganiserEventViewModel>>()
            ?? new List<OrganiserEventViewModel>();

        var today = DateTime.Today;

        ViewBag.UpcomingRaces = events.Count(e => e.EventDate.Date >= today);
        ViewBag.CompletedRaces = events.Count(e => e.EventDate.Date < today);

        var enrolmentsResponse =
            await client.GetAsync("api/Enrolments/my-enrolments");

        if (enrolmentsResponse.IsSuccessStatusCode)
        {
            var enrolments = await enrolmentsResponse.Content.ReadFromJsonAsync<
                List<ParticipantEnrolmentViewModel>>()
                ?? new List<ParticipantEnrolmentViewModel>();

            ViewBag.MyEnrolments = enrolments.Count;
        }
        else
        {
            ViewBag.MyEnrolments = 0;
        }

        // Retrieve the participant's results.
        var resultsResponse =
            await client.GetAsync("api/Results/my-results");

        if (resultsResponse.IsSuccessStatusCode)
        {
            var results = await resultsResponse.Content.ReadFromJsonAsync<
                List<ParticipantResultViewModel>>()
                ?? new List<ParticipantResultViewModel>();

            ViewBag.RecentResults = results
                .OrderByDescending(r => r.EventDate)
                .Take(5)
                .ToList();
        }
        else
        {
            ViewBag.RecentResults =
                new List<ParticipantResultViewModel>();
        }

        events = events
            .Where(e => e.EventDate.Date >= today)
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

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.GetAsync("api/Profile");

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] =
                "Could not retrieve your profile.";

            return View(new ParticipantProfileViewModel());
        }

        var profile = await response.Content.ReadFromJsonAsync<
            ParticipantProfileViewModel>();

        if (profile == null)
        {
            TempData["ErrorMessage"] =
                "Your profile could not be loaded.";

            return View(new ParticipantProfileViewModel());
        }

        return View(profile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(
    ParticipantProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.PutAsJsonAsync(
            "api/Profile",
            model);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();

            TempData["ErrorMessage"] =
                "Profile update failed. " + errorMessage;

            return View(model);
        }

        TempData["SuccessMessage"] =
            "Your profile was updated successfully.";

        return RedirectToAction(nameof(Profile));
    }
}
