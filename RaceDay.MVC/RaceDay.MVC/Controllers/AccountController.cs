using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
}
