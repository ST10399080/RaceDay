using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RaceDay.MVC.Controllers;

[Authorize(Roles = "Organiser")]
public class OrganiserController : Controller
{
    public IActionResult Dashboard()
    {
        return View();
    }
    public IActionResult Events()
    {
        return View();
    }
}