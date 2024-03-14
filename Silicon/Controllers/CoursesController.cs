using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController : Controller
{
    [HttpGet]
    [Route("/courses")]
    public IActionResult Index()
    {
        ViewData["Title"] = "Courses";
        return View();
    }

    [HttpPost]
    public IActionResult SingleCourse()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Bookmark()
    {
        return View();
    }
}
