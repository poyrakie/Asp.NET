using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class AuthController : Controller
{
    [Route("/signup")]
    public IActionResult SignUp()
    {
        ViewData["Title"] = "Sign up";
        return View();
    }
}
