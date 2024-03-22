using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class CourseRepository(DataContext context) : BaseRepo<CourseEntity>(context)
{
    private readonly DataContext _context = context;
}
