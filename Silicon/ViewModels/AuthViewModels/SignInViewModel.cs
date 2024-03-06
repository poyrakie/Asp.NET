using Infrastructure.Models.AuthModels;

namespace Silicon.ViewModels.AuthViewModels;

public class SignInViewModel
{
    public string Title { get; set; } = "Sign in";
    public SignInFormModel Form { get; set; } = new SignInFormModel();
    public string? ErrorMessage { get; set; }
}
