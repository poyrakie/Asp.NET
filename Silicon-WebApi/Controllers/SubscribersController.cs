using Infrastructure.Filters;
using Infrastructure.Models.CourseModels;
using Infrastructure.Models.HomeModels;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Filters;

namespace Silicon_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[UseApiKey]
public class SubscribersController(SubscriberService subscriberService, SubscriberRepository repo) : ControllerBase
{
    private readonly SubscriberService _subscriberService = subscriberService;
    private readonly SubscriberRepository _repo = repo;

    [HttpPost]
    public async Task<IActionResult> Create(NewsletterModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _subscriberService.CreateSubscriberAsync(model);
            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
            {
                return Created("", null);
            }
            else if (result.StatusCode == Infrastructure.Models.StatusCode.EXISTS)
            {
                return Conflict();
            }
        }
        return BadRequest();
    }

    [HttpDelete("{email}")]
    public async Task<IActionResult> Delete(string email)
    {
        var result = await _repo.DeleteAsync(x => x.Email == email);
        if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            return Ok();
        }
        else if (result.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
        {
            return NotFound();
        }
        else return BadRequest();
    }
}
