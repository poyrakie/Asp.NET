using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class UserService(UserRepository repo)
{
    private readonly UserRepository _repo = repo;
}
