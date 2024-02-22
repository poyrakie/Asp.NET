using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels;

namespace Silicon.Controllers;

public class AccountController : Controller
{
    //private readonly AccountService _accountService;

    //public AccountController(AccountService accountService)
    //{
    //    _accountService = accountService;
    //}

    [Route("/account")]
    [HttpGet]
    public IActionResult Details()
    {
        var viewModel = new AccountDetailsViewModel();
        //viewModel.BasicInfo = _accountService.GetBasicInfo();
        //viewModel.AddressInfo = _accountService.GetAddressinfo();
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
}
