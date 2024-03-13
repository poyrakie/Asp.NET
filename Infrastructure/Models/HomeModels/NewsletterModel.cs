using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.HomeModels;

public class NewsletterModel
{
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email address", Prompt = "Your Email", Order = 0)]
    [Required(ErrorMessage = "Email is required")]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be formated 'a@a.aa'")]
    public string Email { get; set; } = null!;

    [Display(Name = "Daily Newsletter", Order = 1)]
    public bool DailyNewsLetter { get; set; }
    [Display(Name = "Event Updates", Order = 2)]
    public bool EventUpdates { get; set; }
    [Display(Name = "Advertising Updates", Order = 3)]
    public bool AdvertisingUpdates { get; set; }
    [Display(Name = "Startups Weekly", Order = 4)]
    public bool StartupsWeekly { get; set; }
    [Display(Name = "Week In Review", Order = 5)]
    public bool WeekInReview { get; set; }
    [Display(Name = "Podcasts", Order = 6)]
    public bool Podcasts { get; set; }


}
