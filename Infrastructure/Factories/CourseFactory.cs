using Infrastructure.Entities;
using Infrastructure.Models.AccountModels;
using Infrastructure.Models.CourseModels;

namespace Infrastructure.Factories;

public class CourseFactory
{
    public CourseEntity PopulateCourseEntity(CourseModel model)
    {
        var courseEntity = new CourseEntity();
        if (model is not null)
        {
            courseEntity.Title = model.Title;
            courseEntity.ImageName = model.ImageName;
            courseEntity.Author = model.Author;
            courseEntity.IsBestSeller = model.IsBestSeller;
            courseEntity.Hours = model.Hours;
            courseEntity.OriginalPrice = model.OriginalPrice;
            courseEntity.DiscountPrice = model.DiscountPrice;
            courseEntity.LikesInProcent = model.LikesInProcent;
            courseEntity.LikesInNumbers = model.LikesInNumbers;
        }
        return courseEntity;
    }
}
