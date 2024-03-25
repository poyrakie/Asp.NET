using Infrastructure.Filters;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.CourseModels;

public class CourseModel
{
    public string Title { get; set; } = null!;
    public string? ImageName { get; set; }
    public string? Author { get; set; }
    public bool IsBestSeller { get; set; }
    public int Hours { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal DiscountPrice { get; set; }
    public decimal LikesInProcent { get; set; }
    public decimal LikesInNumbers { get; set; }
    [Required]
    [ApiKeyValidation]
    public string ApiKey { get; set; } = "e7b38f97-46f2-4e42-8cf2-9e5b6b1b433b";
}
