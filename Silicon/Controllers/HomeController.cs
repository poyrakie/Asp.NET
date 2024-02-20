using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "Welcome";
        return View();
    }
}
