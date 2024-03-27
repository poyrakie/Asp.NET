﻿using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;
using Silicon.ViewModels.CoursesViewModels;

namespace Silicon.Controllers;

[Authorize]
public class CoursesController(UserManager<UserEntity> userManager, SavedCoursesService savedCoursesService, SavedCoursesRepository savedCoursesRepository, CourseService courseService) : Controller
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SavedCoursesService _savedCoursesService = savedCoursesService;
    private readonly SavedCoursesRepository _savedCoursesRepository = savedCoursesRepository;
    private readonly CourseService _courseService = courseService;

    //[HttpGet]
    //[Route("/courses")]
    //public IActionResult Index()
    //{
    //    ViewData["Title"] = "Courses";
    //    return View();
    //}

    [HttpGet]
    [Route("/courses")]
    public async Task<IActionResult> Index()
    {
        var viewModel = new CoursesViewModel();

        var user = await _userManager.GetUserAsync(User);

        var courseListResult = await _courseService.GetCourseListAsync();
        if (courseListResult.StatusCode == Infrastructure.Models.StatusCode.OK)
            viewModel.List = (IEnumerable<CourseEntity>)courseListResult.ContentResult!;

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
    public async Task<IActionResult> SingleCourse(string id)
    {
        var result = await _courseService.GetSingleCourseAsync(id);
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
