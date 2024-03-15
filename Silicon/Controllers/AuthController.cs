using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.AuthViewModels;
using System.Security.Claims;

namespace Silicon.Controllers;

public class AuthController(UserService userService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, UserFactory userFactory) : Controller
{
    private readonly UserService _userService = userService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly UserFactory _userFactory = userFactory;

    #region Signup
    [Route("/signup")]
    [HttpGet]
    public IActionResult SignUp()
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");
        var viewModel = new SignUpViewModel();
        return View(viewModel);
    }

    [Route("/signup")]
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.RegisterUserAsync(model.Form);
            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
                return RedirectToAction("SignIn", "Auth");

            model.ErrorMessage = result.Message;
            return View(model);
        }

        model.ErrorMessage = "Password must be one uppercase, one lowercase, one number, and one special character.And a minimum of 8 characters";
        return View(model);
    }
    #endregion

    #region Signin
    [Route("/signin")]
    [HttpGet]
    public IActionResult SignIn()
    {
        if (_signInManager.IsSignedIn(User))
            return RedirectToAction("Details", "Account");
        var viewModel = new SignInViewModel();
        return View(viewModel);
    }

    [Route("/signin")]
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _userService.SignInUserAsync(model.Form);
            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
                return RedirectToAction("Details", "Account");
            model.ErrorMessage = result.Message;
            return View(model);
        }
        model.ErrorMessage = "You must enter email and password";
        return View(model);
    }
    #endregion

    #region Facebook
    [HttpGet]
    public IActionResult Facebook()
    {
        var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("FacebookCallback"));
        return new ChallengeResult("Facebook", authProps);
    }

    [HttpGet]
    public async Task<IActionResult> FacebookCallback()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info != null)
        {
            var result = await _userService.RegisterOrUpdateExternalAsync(info);
            if (result.StatusCode != Infrastructure.Models.StatusCode.ERROR)
            {
                var user = (UserEntity)result.ContentResult!;
                await _signInManager.SignInAsync(user, isPersistent: false);
                if (HttpContext.User != null)
                {
                    return RedirectToAction("Details", "Account");
                }
            }
        }
        ModelState.AddModelError("InvalidFacebookAuthentication", "Failed to authenticate with Facebook");
        return RedirectToAction("SignIn", "Auth");

    }
    #endregion

    #region Google
    [HttpGet]
    public IActionResult Google()
    {
        var authProps = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("GoogleCallback"));
        return new ChallengeResult("Google", authProps);
    }

    [HttpGet]
    public async Task<IActionResult> GoogleCallBack()
    {
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info != null)
        {
            var result = await _userService.RegisterOrUpdateExternalAsync(info);
            if (result.StatusCode != Infrastructure.Models.StatusCode.ERROR)
            {
                var user = (UserEntity)result.ContentResult!;
                await _signInManager.SignInAsync(user, isPersistent: false);
                if (HttpContext.User != null)
                {
                    return RedirectToAction("Details", "Account");
                }
            }
        }
        ModelState.AddModelError("InvalidFacebookAuthentication", "Failed to authenticate with Facebook");
        return RedirectToAction("SignIn", "Auth");

    }
    #endregion
}
