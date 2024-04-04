using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.HomeViewModels;

namespace Silicon.Controllers;

public class HomeController(SubscriberService subscriberService) : Controller
{
    private readonly SubscriberService _subscriberService = subscriberService;

    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new HomeViewModel();
        if (TempData.ContainsKey("DisplayMessage")) 
        {
            viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();
        }
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Subscribe(HomeViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var result = await _subscriberService.ApiCallCreateSubscriberAsync(viewModel.Newsletter);
            TempData["DisplayMessage"] = result.Message; 
        }
        return RedirectToAction("Index", "Home", "newsletter");
    }
}
