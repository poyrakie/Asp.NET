using Infrastructure.Entities;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.AccountViewModels;
using Infrastructure.Services;
using Infrastructure.Models.AccountModels;

namespace Silicon.Controllers;

[Authorize]
public class AccountController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, AccountFactory accountFactory, AddressService addressService, UserFactory userFactory) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AccountFactory _accountFactory = accountFactory;
    private readonly AddressService _addressService = addressService;
    private readonly UserFactory _userFactory = userFactory;


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

    [Route("/account")]
    [HttpPost]
    public async Task<IActionResult> Details(AccountDetailsViewModel viewModel)
    {
        var userEntity = await _userManager.GetUserAsync(User);
        if (ValidateBasicInfo(viewModel.BasicInfo))
        {
            var result = await _accountFactory.UpdateBasicInfoAsync(viewModel.BasicInfo, userEntity!);
            if (result.StatusCode == Infrastructure.Models.StatusCode.ERROR)
            {

            }
        }

        //if (ModelState.ContainsKey("BasicInfo"))
        //{
        //    foreach (var entry in ModelState["BasicInfo"]!.Children!)
        //    {
        //        if (entry.ValidationState == ModelValidationState.Invalid)
        //        {
        //            basicBool = false;
        //            break;
        //        }
        //    }

        //    if (basicBool)
        //    {

        //    }
        //}

        //else if (ModelState["AddressInfo"]!.ValidationState == ModelValidationState.Valid)
        //{
        //    var result = await _accountFactory.UpdateAddressInfoAsync(viewModel.AddressInfo, userEntity!);
        //    if (result.StatusCode == Infrastructure.Models.StatusCode.ERROR)
        //    {

        //    }
        //}
        userEntity = await _userManager.GetUserAsync(User);
        viewModel.BasicInfo = _accountFactory.PopulateBasicInfo(userEntity!);
        viewModel.AddressInfo = _accountFactory.PopulateAddressInfo(userEntity!);
        return View(viewModel);
    }


    [Route("/signout")]
    [HttpGet]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
