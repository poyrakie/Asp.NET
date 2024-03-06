using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class AddressService(AddressRepository repo)
{
    private readonly AddressRepository _repo = repo;
}
