using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Filters;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class ContactController(ContactService contactService) : ControllerBase
{
    private readonly ContactService _contactService = contactService;

    [HttpPost]
    public async Task<IActionResult> Create(ContactFormModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _contactService.CreateAsync(model);
            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
            {
                return Created("", null);
            }
        }
        return BadRequest();
    }
}
