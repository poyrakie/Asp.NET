using Infrastructure.Entities;
using Infrastructure.Models.AuthModels;

namespace Silicon.ViewModels.CoursesViewModels;

public class CoursesViewModel
{
    public string Title { get; set; } = "Courses";
    public IEnumerable<CourseEntity>? List { get; set; }
    public IEnumerable<SavedCoursesEntity>? SavedList { get; set; }
    public string? DisplayMessage { get; set; }
}
