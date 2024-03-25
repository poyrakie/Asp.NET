using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Silicon.ViewModels.CoursesViewModels;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController : Controller
{
    //[HttpGet]
    //[Route("/courses")]
    //public IActionResult Index()
    //{

    //    ViewData["Title"] = "Courses";
    //    return View();
    //}

    [HttpGet]
    [Route("/courses")]
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Courses";
        using var http = new HttpClient();
        var response = await http.GetAsync("https://localhost:7126/api/courses/getall?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b");
        var json = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<IEnumerable<CourseEntity>>(json);
        var viewModel = new CoursesViewModel
        {
            List = data
        };
        return View(viewModel);
    }

    [HttpGet]
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
