using Infrastructure.Entities;
using Infrastructure.Models.HomeModels;

namespace Infrastructure.Factories;

public class SubscriberFactory
{
    public SubscriberEntity PopulateSubscriberEntity(NewsletterModel model)
    {
        var entity = new SubscriberEntity();
        if (model != null)
        {
            entity.Email = model.Email;
            entity.DailyNewsLetter = model.DailyNewsLetter;
            entity.EventUpdates = model.EventUpdates;
            entity.AdvertisingUpdates = model.AdvertisingUpdates;
            entity.StartupsWeekly = model.StartupsWeekly;
            entity.WeekInReview = model.WeekInReview;
            entity.Podcasts = model.Podcasts;
        }
        return entity;
    }
}