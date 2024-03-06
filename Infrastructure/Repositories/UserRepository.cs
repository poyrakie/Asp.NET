using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Migrations;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public  class UserRepository(DataContext context) : BaseRepo<UserEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            IEnumerable<UserEntity> list = await _context.Set<UserEntity>()
                .Include(i => i.Address)
                .ToListAsync();

            if (!list.Any())
                return ResponseFactory.NotFound("List is empty u fuck");
            return ResponseFactory.Ok(list);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public override async Task<ResponseResult> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Set<UserEntity>()
                .Include(i => i.Address)
                .FirstOrDefaultAsync(predicate);

            if (entity == null)
                return ResponseFactory.NotFound();
            return ResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
