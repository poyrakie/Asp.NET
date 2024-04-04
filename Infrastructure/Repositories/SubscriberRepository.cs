using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class SubscriberRepository(DataContext context) : BaseRepo<SubscriberEntity> (context)
{
    private readonly DataContext _context = context;
}

