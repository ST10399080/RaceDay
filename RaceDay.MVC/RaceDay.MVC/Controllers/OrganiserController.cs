using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;

namespace RaceDay.MVC.Controllers;

[Authorize(Roles = "Organiser")]
public class OrganiserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OrganiserController(IHttpClientFactory httpClientFactory)
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
                "Could not retrieve events from the RaceDay API.";

            return View(new List<OrganiserEventViewModel>());
        }

        var events = await response.Content.ReadFromJsonAsync<
            List<OrganiserEventViewModel>>() ?? new List<OrganiserEventViewModel>();

        var organiserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        events = events
            .Where(e => e.OrganiserID.ToString() == organiserId)
            .OrderBy(e => e.EventDate)
            .ToList();

        return View(events);
    }

    [HttpGet]
    public IActionResult CreateEvent()
    {
        return View(new CreateEventViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
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


        var eventRequest = new
        {
            model.EventName,
            model.Description,
            model.EventDate,
            model.Location,
            model.Distance,
            model.EventType,
            BannerImageURL = (string?)null
        };

        var response = await client.PostAsJsonAsync(
            "api/Events",
            eventRequest);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "The event could not be created. Please try again.");

            return View(model);
        }

        using var responseContent =
            await response.Content.ReadAsStreamAsync();

        using var jsonDocument =
            await JsonDocument.ParseAsync(responseContent);

        var eventId = jsonDocument.RootElement
            .GetProperty("eventID")
            .GetInt32();

        if (model.BannerImage != null &&
            model.BannerImage.Length > 0)
        {
            using var multipartContent = new MultipartFormDataContent();

            await using var imageStream = model.BannerImage.OpenReadStream();

            using var imageContent = new StreamContent(imageStream);

            imageContent.Headers.ContentType =
                new MediaTypeHeaderValue(model.BannerImage.ContentType);

            multipartContent.Add(
                imageContent,
                "file",
                model.BannerImage.FileName);

            var uploadResponse = await client.PostAsync(
                $"api/Events/{eventId}/banner",
                multipartContent);

            if (!uploadResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] =
                    "The event was created, but its banner image could not be uploaded.";

                return RedirectToAction(nameof(Events));
            }
        }

        TempData["SuccessMessage"] = "Event created successfully!";

        return RedirectToAction(nameof(Events));
    }

    [HttpGet]
    public async Task<IActionResult> EditEvent(int id)
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
            TempData["ErrorMessage"] = "Could not retrieve the event.";
            return RedirectToAction(nameof(Events));
        }

        var events = await response.Content.ReadFromJsonAsync<
            List<OrganiserEventViewModel>>() ?? new List<OrganiserEventViewModel>();

        var organiserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var selectedEvent = events.FirstOrDefault(e =>
            e.EventID == id &&
            e.OrganiserID.ToString() == organiserId);

        if (selectedEvent == null)
        {
            TempData["ErrorMessage"] =
                "Event not found or you do not have permission to edit it.";

            return RedirectToAction(nameof(Events));
        }

        var model = new EditEventViewModel
        {
            EventID = selectedEvent.EventID,
            EventName = selectedEvent.EventName,
            Description = selectedEvent.Description,
            EventDate = selectedEvent.EventDate,
            Location = selectedEvent.Location,
            Distance = selectedEvent.Distance,
            EventType = selectedEvent.EventType
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEvent(
        int id,
        EditEventViewModel model)
    {
        if (id != model.EventID)
        {
            return BadRequest();
        }

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

        var eventRequest = new
        {
            model.EventName,
            model.Description,
            model.EventDate,
            model.Location,
            model.Distance,
            model.EventType,
            BannerImageURL = (string?)null
        };

        var response = await client.PutAsJsonAsync(
            $"api/Events/{id}",
            eventRequest);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "The event could not be updated. Please try again.");

            return View(model);
        }

        if (model.BannerImage != null &&
            model.BannerImage.Length > 0)
        {
            using var multipartContent = new MultipartFormDataContent();

            await using var imageStream =
                model.BannerImage.OpenReadStream();

            using var imageContent = new StreamContent(imageStream);

            imageContent.Headers.ContentType =
                new MediaTypeHeaderValue(model.BannerImage.ContentType);

            multipartContent.Add(
                imageContent,
                "file",
                model.BannerImage.FileName);

            var uploadResponse = await client.PostAsync(
                $"api/Events/{id}/banner",
                multipartContent);

            if (!uploadResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] =
                    "Event details were updated, but the banner could not be replaced.";

                return RedirectToAction(nameof(Events));
            }
        }

        TempData["SuccessMessage"] = "Event updated successfully!";

        return RedirectToAction(nameof(Events));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrWhiteSpace(accessToken))
        {
            return RedirectToAction("Login", "Account");
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await client.DeleteAsync($"api/Events/{id}");

        if (!response.IsSuccessStatusCode)
        {
            TempData["ErrorMessage"] =
                "The event could not be deleted. Please try again.";

            return RedirectToAction(nameof(Events));
        }

        TempData["SuccessMessage"] = "Event deleted successfully!";

        return RedirectToAction(nameof(Events));
    }
}