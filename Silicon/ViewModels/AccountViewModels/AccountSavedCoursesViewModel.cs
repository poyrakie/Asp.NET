using Infrastructure.Entities;
using Infrastructure.Models.AccountModels;

namespace Silicon.ViewModels.AccountViewModels;

public class AccountSavedCoursesViewModel
{
    public string Title { get; set; } = "Saved courses";
    public AccountDetailsBasicInfoModel BasicInfo { get; set; } = new AccountDetailsBasicInfoModel();
    public string? DisplayMessage { get; set; }
    public IEnumerable<CourseEntity> List { get; set; } = [];
    public IEnumerable<SavedCoursesEntity> SavedList { get; set; } = [];
}
