using Infrastructure.Models.CourseModels;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Silicon_WebApi.Controllers;

[Route("api/courses")]
[ApiController]
public class CoursesController(CourseService courseService) : ControllerBase
{
    private readonly CourseService _courseService = courseService;


    //[HttpGet]
    //public IActionResult GetAll()
    //{

    //    return Ok();
    //}

    [HttpPost]
    public async Task<IActionResult> Create(CourseModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _courseService.CreateCourseAsync(model);
            if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
            {
                return Created();
            }
            else if (result.StatusCode == Infrastructure.Models.StatusCode.EXISTS)
            {
                return Conflict();
            }
        }
        return BadRequest();
    }
}
