using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.CourseModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Infrastructure.Services;

public class CourseService(CourseRepository courseRepository, CourseFactory courseFactory)
{
    private readonly CourseRepository _courseRepository = courseRepository;
    private readonly CourseFactory _courseFactory = courseFactory;

    public async Task<ResponseResult> CreateCourseAsync(CourseModel model)
    {
        try
        {
            var existsResult = await _courseRepository.ExistsAsync(x => x.Title == model.Title);
            if (existsResult.StatusCode == StatusCode.EXISTS)
            {
                return ResponseFactory.Exists();
            }
            else if (existsResult.StatusCode == StatusCode.NOT_FOUND)
            {
                var courseEntity = _courseFactory.PopulateCourseEntity(model);
                if (courseEntity != null)
                {
                    var createResult = await _courseRepository.CreateAsync(courseEntity);
                    if(createResult.StatusCode == StatusCode.OK)
                    {
                        return ResponseFactory.Ok("Course created successfully");
                    }
                }
            }
            return ResponseFactory.Error("Something went wrong you fuck");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
