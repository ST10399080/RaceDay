using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var client = _httpClientFactory.CreateClient("RaceDayAPI");

        var response = await client.PostAsJsonAsync(
            "api/Auth/login",
            new
            {
                model.Email,
                model.Password
            });

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid email or password.");

            return View(model);
        }

        var loginResponse =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginResponse == null ||
            string.IsNullOrWhiteSpace(loginResponse.AccessToken) ||
            loginResponse.User == null)
        {
            ModelState.AddModelError(
                string.Empty,
                "The API returned an invalid login response.");

            return View(model);
        }

        // Temporary confirmation while we implement cookie authentication.
        // Do not display or expose the JWT to the browser.
        return Content(
            $"API login successful for {loginResponse.User.FirstName}. " +
            "MVC session authentication has not been configured yet.");
    }
}
