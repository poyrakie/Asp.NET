using Infrastructure.Entities;
using Infrastructure.Filters;
using Infrastructure.Models.CourseModels;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Silicon_WebApi.Filters;

namespace Silicon_WebApi.Controllers;

[Route("api/courses")]
[ApiController]

public class CoursesController(CourseService courseService, CourseRepository courseRepository) : ControllerBase
{
    private readonly CourseService _courseService = courseService;
    private readonly CourseRepository _courseRepository = courseRepository;

    [UseApiKey]
    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAll()
    {

        var result = await _courseRepository.GetAllAsync();
        if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            return Ok((IEnumerable<CourseEntity>)result.ContentResult!);
        }
        else if (result.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
        {
            return NotFound();
        }
        return BadRequest();
    }

    [UseApiKey]
    [HttpGet]
    [Route("getone/{id}")]
    public async Task<IActionResult> Get(string id)
    {

        var result = await _courseRepository.GetAsync(x => x.Id == id);
        if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            return Ok((CourseEntity)result.ContentResult!);
        }
        else if (result.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
        {
            return NotFound();
        }
        return BadRequest();
    }

    [Authorize]
    [ApiKeyValidation]
    [HttpPost]
    public async Task<IActionResult> Create(CourseModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _courseService.CreateCourseAsync(model);
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

    [Authorize]
    [UseApiKey]
    [HttpPut]
    public async Task<IActionResult> Update(CourseEntity entity)
    {
        if (ModelState.IsValid)
        {
            var result = await _courseRepository.UpdateAsync((x => x.Id == entity.Id), entity);
            if(result.StatusCode == Infrastructure.Models.StatusCode.OK)
            {
                return Ok((CourseEntity)result.ContentResult!);
            }
            else if(result.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
            {
                return NotFound();
            }
        }
        return BadRequest();
    }

    [Authorize]
    [UseApiKey]
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _courseRepository.DeleteAsync((x) => x.Id == id);
        if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            return Ok(null);
        }
        else if (result.StatusCode == Infrastructure.Models.StatusCode.NOT_FOUND)
        {
            return NotFound();
        }
        return BadRequest();
    }
}
