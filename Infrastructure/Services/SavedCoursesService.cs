using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class SavedCoursesService(SavedCoursesRepository savedCoursesRepository, SavedCoursesFactory savedCoursesFactory)
{

    private readonly SavedCoursesRepository _savedCoursesRepository = savedCoursesRepository;
    private readonly SavedCoursesFactory _savedCoursesFactory = savedCoursesFactory;

    public async Task<ResponseResult> CreateOrDeleteBookmarkAsync(string courseId, UserEntity user)
    {
        try
        {
            Expression<Func<SavedCoursesEntity, bool>> expression = x => x.CourseId == courseId && x.UserId == user.Id;
            var result = await _savedCoursesRepository.ExistsAsync(expression);

            if (result.StatusCode == StatusCode.EXISTS)
            {
                var deleteResult = await _savedCoursesRepository.DeleteAsync(expression);
                if (deleteResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok("Bookmark removed");
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
            {
                var entity = _savedCoursesFactory.PopulateSavedCourseEntity(courseId, user);
                var createResult = await _savedCoursesRepository.CreateAsync(entity);
                if (createResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok("Bookmark saved");

            }
            return ResponseFactory.Error();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> CreateSavedList(UserEntity user)
    {
        try
        {
            var listresult = await _savedCoursesRepository.GetAllAsync();
            if (listresult.StatusCode == StatusCode.OK)
            {
                var fullList = (IEnumerable<SavedCoursesEntity>)listresult.ContentResult!;
                var userList = new List<SavedCoursesEntity>();
                foreach (var item in fullList)
                {
                    if (item.UserId == user!.Id)
                    {
                        userList.Add(item);
                    }
                }
                return ResponseFactory.Ok(userList);
            }
            else
            {
                return ResponseFactory.NotFound();
            }
        }
        catch (Exception ex){ return ResponseFactory.Error(ex.Message); }

    }
}
