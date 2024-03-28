using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.ContactViewModels;

namespace Silicon.Controllers;

public class ContactController(ContactService contactService) : Controller
{
    private readonly ContactService _contactService = contactService;

    [Route("/contact")]
    [HttpGet]
    public IActionResult Index()
    {
        var viewModel = new ContactViewModel();
        if (TempData.ContainsKey("DisplayMessage"))
        {
            viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();
        }
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> SendRequest(ContactViewModel viewModel)
    {
        if(ModelState.IsValid)
        {
            
            var result = await _contactService.ApiCallCreateSendRequestAsync(viewModel.ContactForm);
            TempData["DisplayMessage"] = result.Message;
        }

        return RedirectToAction("Index");
    }
}
