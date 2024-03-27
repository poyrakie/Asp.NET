using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.CourseModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

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
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> GetCourseListAsync()
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync("https://localhost:7126/api/courses/getall?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<CourseEntity>>(json);
            if (data != null)
                return ResponseFactory.Ok(data);
            return ResponseFactory.NotFound();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> GetSingleCourseAsync(string id)
    {
        try
        {
            using var http = new HttpClient();
            var response = await http.GetAsync($"https://localhost:7126/api/courses/getone/{id}?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b");
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CourseEntity>(json);
            if (data != null)
                return ResponseFactory.Ok(data);
            return ResponseFactory.NotFound();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
