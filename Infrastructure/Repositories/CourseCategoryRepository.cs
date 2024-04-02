using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class CourseCategoryRepository(DataContext context) : BaseRepo<CourseCategoryEntity>(context)
{
    private readonly DataContext _context = context;
}