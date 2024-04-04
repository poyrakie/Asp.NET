using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.CourseModels;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Silicon.ViewModels.CoursesViewModels;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController(UserManager<UserEntity> userManager, SavedCoursesService savedCoursesService, CourseService courseService, CategoryRepository categoryRepository) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SavedCoursesService _savedCoursesService = savedCoursesService;
    private readonly CategoryRepository _categoryRepository = categoryRepository;
    private readonly CourseService _courseService = courseService;


    [HttpGet]
    [Route("/courses")]
    public async Task<IActionResult> Index(string category = "", string searchQuery = "", int pageNumber = 1, int pageSize = 3)
    {
        var viewModel = new CoursesViewModel();

        var user = await _userManager.GetUserAsync(User);
        var categoryResult = await _categoryRepository.GetAllAsync();
        if (categoryResult.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            viewModel.CategoryList = (IEnumerable<CategoryEntity>)categoryResult.ContentResult!;
        }

        var courseListResult = await _courseService.ApiCallGetCourseListAsync(category, searchQuery, pageNumber, pageSize);
        if (courseListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            var courseResult = (CourseResultModel)courseListResult.ContentResult!;
            viewModel.Pagination = new PaginationModel
            {
                TotalPages = courseResult.TotalPages,
                TotalItems = courseResult.TotalItems,
                PageSize = pageSize,
                PageNumber = pageNumber
            };
            viewModel.CourseList = courseResult.Courses!;
        }

        var savedListResult = await _savedCoursesService.CreateSavedList(user!);
        if (savedListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            viewModel.SavedList = (IEnumerable<SavedCoursesEntity>)savedListResult.ContentResult!;

        if (TempData.ContainsKey("DisplayMessage"))
        {
            viewModel.DisplayMessage = TempData["DisplayMessage"]!.ToString();
        }
        return View(viewModel);
    }

    [HttpGet]
    [Route("/singlecourse")]
    public async Task<IActionResult> SingleCourse(string id)
    {
        var result = await _courseService.ApiCallGetSingleCourseAsync(id);
        if (result.StatusCode == Infrastructure.Models.StatusCode.OK)
        {
            return View((CourseEntity)result.ContentResult!);
        }
        TempData["DisplayMessage"] = "Something went wrong";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Bookmark(string id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var result = await _savedCoursesService.CreateOrDeleteBookmarkAsync(id, user);
            TempData["DisplayMessage"] = result.Message;
        }
        return RedirectToAction("Index");
    }
}
