using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.CourseModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Services;

public class CourseService(CourseRepository courseRepository, CourseFactory courseFactory, DataContext context)
{
    private readonly CourseRepository _courseRepository = courseRepository;
    private readonly CourseFactory _courseFactory = courseFactory;
    private readonly DataContext _context = context;

    public async Task<ResponseResult> GetAllCoursesWithFiltersAsync(string category, string searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            var query = _context.Courses
                .Include(i => i.CourseCategories)!
                    .ThenInclude(c => c.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(category) && category != "all")
            {
                query = query.Where(x => x.CourseCategories!.Any(cc => cc.Category.Name == category));
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query = query.Where(x => x.Title.Contains(searchQuery) || x.Author!.Contains(searchQuery));
            }
            query = query.OrderByDescending(x => x.Title);

            var response = new CourseResultModel
            {
                TotalItems = await query.CountAsync(),
                
            };
            response.TotalPages = (int)Math.Ceiling(response.TotalItems / (double)pageSize);
            response.Courses = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return ResponseFactory.Ok(response);
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

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
                    if (createResult.StatusCode == StatusCode.OK)
                    {
                        return ResponseFactory.Ok("Course created successfully");
                    }
                }
            }
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> ApiCallGetCourseListAsync()
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

    public async Task<ResponseResult> ApiCallGetCourseListAsync(string category, string searchQuery, int pageNumber, int pageSize)
    {
        try
        {
            using var http = new HttpClient();
            var queryParameters = new Dictionary<string, string>
            {
                { "key", "e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b" },
                { "category", Uri.EscapeDataString(category) },
                { "searchQuery", Uri.EscapeDataString(searchQuery) },
                { "pageNumber", pageNumber.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            var urlBuilder = new StringBuilder("https://localhost:7126/api/courses/getallwithfilters?");
            foreach (var param in queryParameters)
            {
                urlBuilder.Append($"{param.Key}={param.Value}&");
            }
            var url = urlBuilder.ToString().TrimEnd('&');

            var response = await http.GetAsync(url);

            //var response = await http.GetAsync($"https://localhost:7126/api/courses/getall?key=e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b&category={Uri.UnescapeDataString(category)}&searchQuery={Uri.UnescapeDataString(searchQuery)}&pageNumber={pageNumber}&pageSize={pageSize}");
            //var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CourseResultModel>(await response.Content.ReadAsStringAsync());
            if (data!.Courses != null)
                return ResponseFactory.Ok(data);
            return ResponseFactory.NotFound();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

    public async Task<ResponseResult> ApiCallGetSingleCourseAsync(string id)
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
