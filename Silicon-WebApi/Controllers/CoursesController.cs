using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Models;

namespace Silicon_WebApi.Controllers;

[Route("api/courses")]
[ApiController]
public class CoursesController : ControllerBase
{
    private List<CoursesModel> _courses = [];

    [HttpGet]
    public IActionResult GetAll()
    {

        return Ok();
    }

    [HttpPost]
    public IActionResult Create(CoursesModel model)
    {
        if (ModelState.IsValid)
        {
            return Created("", model);
        }
        return BadRequest();
    }
}
