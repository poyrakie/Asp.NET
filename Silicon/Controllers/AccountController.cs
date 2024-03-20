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

    #region Account
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
        var externalUser = await _signInManager.GetExternalLoginInfoAsync();
        var claims = HttpContext.User.Identities.FirstOrDefault();
        TempData["BasicDisplayMessage"] = "You must fill out all neccessary fields";
        if (externalUser is not null)
        {
            var result = await _userService.UpdateExternalAsync(basicForm, externalUser, claims!);
            TempData["BasicDisplayMessage"] = result.Message;
        }
        else if (TryValidateModel(basicForm))
        {
            var userEntity = await _userManager.GetUserAsync(User);
            if (userEntity is not null)
            {
                var result = await _userService.UpdateBasicInfoAsync(basicForm, userEntity);
                TempData["BasicDisplayMessage"] = result.Message;
            }
        }
        return RedirectToAction("Details");
    }


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
    #endregion

    [Route("/signout")]
    [HttpGet]
    public new async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    #region Security
    [Route("/security")]
    [HttpGet]
    public async Task<IActionResult> Security()
    {
        var userEntity = await _userManager.GetUserAsync(User);
        var viewModel = new AccountSecurityViewModel();
        if (userEntity != null)
        {
            viewModel.BasicInfo = _accountFactory.PopulateBasicInfo(userEntity!);
        }
        if (TempData.ContainsKey("PasswordDisplayMessage"))
        {
            viewModel.PasswordDisplayMessage = TempData["PasswordDisplayMessage"]!.ToString();
        }
        if (TempData.ContainsKey("DeleteDisplayMessage"))
        {
            viewModel.DeleteDisplayMessage = TempData["DeleteDisplayMessage"]!.ToString();
        }


        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([Bind(Prefix = "Password")] AccountSecurityPasswordModel passwordForm)
    {
        var userEntity = await _userManager.GetUserAsync(User);

        TempData["PasswordDisplayMessage"] = "You must fill out all neccessary fields correctly";

        if (TryValidateModel(passwordForm))
        {
            if (userEntity != null)
            {
                var result = await _userManager.ChangePasswordAsync(userEntity, passwordForm.CurrentPassword, passwordForm.NewPassword);

                if (result.Succeeded)
                {
                    TempData["PasswordDisplayMessage"] = "Updated password successfully";
                }
            }
        }
        return RedirectToAction("Security");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteAccount([Bind(Prefix = "Delete")] AccountSecurityDeleteModel delete)
    {
        var userEntity = await _userManager.GetUserAsync(User);

        if (TryValidateModel(delete))
        {
            if (userEntity != null)
            {
                var result = await _userManager.DeleteAsync(userEntity);
                if (result.Succeeded)
                {
                    return RedirectToAction("SignOut");
                }
            }
        }
        TempData["DeleteDisplayMessage"] = "You must confirm the deletion of your account";
        return RedirectToAction("Security");
    }
    #endregion


    #region SavedCourses

    public async Task<IActionResult> SavedCourses()
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
    #endregion
}
