using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Entities;

[PrimaryKey("Id")]
public class CategoryEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = null!;
    public virtual ICollection<CourseCategoryEntity>? CourseCategories { get; set; }
}
