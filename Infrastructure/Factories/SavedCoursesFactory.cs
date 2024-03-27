using Infrastructure.Entities;

namespace Infrastructure.Factories;

public class SavedCoursesFactory
{
    public SavedCoursesEntity PopulateSavedCourseEntity(string courseId, UserEntity user)
    {
        var entity = new SavedCoursesEntity { CourseId = courseId, UserId = user.Id };
        return entity;
    }
}
