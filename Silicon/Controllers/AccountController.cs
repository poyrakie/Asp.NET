using Infrastructure.Entities;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.AccountViewModels;

namespace Silicon.Controllers;

[Authorize]
public class AccountController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, AccountFactory accountFactory) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AccountFactory _accountFactory = accountFactory;


    [Route("/account")]
    [HttpGet]
    public async Task<IActionResult> Details()
    {

        var userEntity = await _userManager.GetUserAsync(User);
        var viewModel = new AccountDetailsViewModel();
        if (userEntity != null)
        {
            viewModel.BasicInfo = _accountFactory.PopulateBasicInfo(userEntity!);
            viewModel.AddressInfo = _accountFactory.PopulateAddressInfo(userEntity!);
        }

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult BasicInfo(AccountDetailsViewModel viewModel)
    {
        // _accountService.SaveBasicInfo(viewModel.BasicInfo);
        return RedirectToAction("Details", "Account");
    }

    [HttpPost]
    public IActionResult AddressInfo(AccountDetailsViewModel viewModel)
    {
        // _accountService.SaveAddressInfo(viewModel.AddressInfo);
        return RedirectToAction("Details", "Account");
    }

    [Route("/signout")]
    [HttpGet]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
