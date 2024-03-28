using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class SubscriberEntity
{
    [Key]
    [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
    public string Email { get; set; } = null!;
    public bool DailyNewsLetter { get; set; }
    public bool EventUpdates { get; set; }
    public bool AdvertisingUpdates { get; set; }
    public bool StartupsWeekly { get; set; }
    public bool WeekInReview { get; set; }
    public bool Podcasts { get; set; }
}
