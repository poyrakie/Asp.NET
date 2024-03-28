using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class ContactFormModel
{
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address", Prompt = "Enter your email address", Order = 0)]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be formated 'a@a.aa'")]
    public string Email { get; set; } = null!;

    [Display(Name = "Full name", Prompt = "Enter your full name", Order = 1)]
    [Required(ErrorMessage = "Full name is required")]
    [MinLength(2, ErrorMessage = "Full name is required")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Services", Prompt = "Choose the service you are interested in", Order = 2)]
    public string? Service { get; set; }

    [Display(Name = "Message", Prompt = "Enter your message", Order = 3)]
    [Required(ErrorMessage = "Message is required")]
    [MinLength(2, ErrorMessage = "Message is required")]
    public string Message { get; set; } = null!;
}
