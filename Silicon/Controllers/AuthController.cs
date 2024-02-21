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

    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn()
    {
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public IActionResult SignIn(SignInViewModel model)
    {
        if (!ModelState.IsValid)
        {

            return View(model);
        }

        //var result = _authService.SignIn(model.Form);
        //if (result)
        //{ return RedirectToAction("Account", "Index"); }

        model.ErrorMessage = "Incorrect email or password";
        return View(model);
    }
}
