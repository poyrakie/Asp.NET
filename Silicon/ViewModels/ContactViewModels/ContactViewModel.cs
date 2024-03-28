using Infrastructure.Models;

namespace Silicon.ViewModels.ContactViewModels;

public class ContactViewModel
{
    public string Title { get; set; } = "Contact Us";
    public ContactFormModel ContactForm { get; set; } = new ContactFormModel();
    public string? DisplayMessage { get; set; }
}
