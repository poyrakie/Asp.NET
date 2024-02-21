using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels;

namespace Silicon.Controllers;

public class AuthController : Controller
{
    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public IActionResult SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ErrorMessage = "Password must be one uppercase, one lowercase, one number, and one special character.And a minimum of 8 characters";
            return View(model);
        }
        return RedirectToAction("SignIn", "Auth");
    }
}
