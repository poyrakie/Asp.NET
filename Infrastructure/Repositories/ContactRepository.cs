using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class ContactRepository(DataContext context) : BaseRepo<ContactEntity>(context)
{
    private readonly DataContext _context = context;
}
