using Infrastructure.Models.HomeModels;

namespace Silicon.ViewModels.HomeViewModels;

public class HomeViewModel
{
    public string Title { get; set; } = "Home";
    public NewsletterModel Newsletter { get; set; } = new NewsletterModel();
    public string? DisplayMessage { get; set; }
}
