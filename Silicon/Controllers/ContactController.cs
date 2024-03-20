using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {

        return View();
    }
}
