using Infrastructure.Models.AccountModels;

namespace Silicon.ViewModels.AccountViewModels;

public class AccountSecurityViewModel
{
    public string Title { get; set; } = "Security";
    public string? PasswordDisplayMessage { get; set; }
    public string? DeleteDisplayMessage { get; set; }
    public AccountDetailsBasicInfoModel BasicInfo { get; set; } = null!;
    public AccountSecurityPasswordModel Password { get; set; } = null!;
    public AccountSecurityDeleteModel Delete { get; set; } = null!;
}
