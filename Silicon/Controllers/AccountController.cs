using Infrastructure.Entities;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.AccountViewModels;
using Infrastructure.Models.AccountModels;
using Infrastructure.Services;

namespace Silicon.Controllers;

[Authorize]
public class AccountController(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, AccountFactory accountFactory, UserFactory userFactory, UserService userService, AddressService addressService) : Controller
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly UserService _userService = userService;
    private readonly AddressService _addressService = addressService;
    private readonly AccountFactory _accountFactory = accountFactory;
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
            viewModel.AddressInfo = await _accountFactory.PopulateAddressInfoAsync(userEntity!);
        }
        if (TempData.ContainsKey("BasicDisplayMessage"))
        {
            viewModel.BasicDisplayMessage = TempData["BasicDisplayMessage"]!.ToString();
        }
        if (TempData.ContainsKey("AddressDisplayMessage"))
        {
            viewModel.AddressDisplayMessage = TempData["AddressDisplayMessage"]!.ToString();
        }

        return View(viewModel);
    }

    [HttpPost]
    [Route("/account/update-basic-info")]
    public async Task<IActionResult> SaveBasicInfo([Bind(Prefix = "BasicInfo")] AccountDetailsBasicInfoModel basicForm)
    {
        var userEntity = await _userManager.GetUserAsync(User);
        TempData["BasicDisplayMessage"] = "You must fill out all neccessary fields";
        if (userEntity != null)
        {
            if (TryValidateModel(basicForm))
            {
                var result = await _userService.UpdateBasicInfoAsync(basicForm, userEntity);
                TempData["BasicDisplayMessage"] = result.Message;
            }
        }
        return RedirectToAction("Details");
    }

    // 

    [HttpPost]
    [Route("/account/update-address-info")]
    public async Task<IActionResult> SaveAddressInfo([Bind(Prefix = "AddressInfo")] AccountDetailsAddressInfoModel addressForm)
    {
        var userEntity = await _userManager.GetUserAsync(User);
        TempData["AddressDisplayMessage"] = "You must fill out all neccessary fields";
        if (userEntity != null)
        {
            if (TryValidateModel(addressForm))
            {
                var result = await _addressService.UpdateAddressInfoAsync(addressForm, userEntity)!;
                TempData["AddressDisplayMessage"] = result.Message;
            }
        }
        return RedirectToAction("Details");
    }

    [Route("/signout")]
    [HttpGet]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
