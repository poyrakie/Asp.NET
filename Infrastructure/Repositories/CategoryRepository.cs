using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class CategoryRepository(DataContext context) : BaseRepo<CategoryEntity>(context)
{
    private readonly DataContext _context = context;
}
