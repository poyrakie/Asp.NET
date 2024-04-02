using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.AuthModels;

namespace Silicon.ViewModels.CoursesViewModels;

public class CoursesViewModel
{
    public string Title { get; set; } = "Courses";
    public PaginationModel? Pagination { get; set; }
    public IEnumerable<CategoryEntity>? CategoryList { get; set; }
    public IEnumerable<CourseEntity>? CourseList { get; set; }
    public IEnumerable<SavedCoursesEntity>? SavedList { get; set; }
    public string? DisplayMessage { get; set; }
}
