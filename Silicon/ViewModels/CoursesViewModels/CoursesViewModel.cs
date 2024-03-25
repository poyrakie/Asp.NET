using Infrastructure.Entities;
using Infrastructure.Models.AuthModels;

namespace Silicon.ViewModels.CoursesViewModels;

public class CoursesViewModel
{
    public string Title { get; set; } = "Sign up";
    public IEnumerable<CourseEntity>? List { get; set; }
    public string? ErrorMessage { get; set; }
}
