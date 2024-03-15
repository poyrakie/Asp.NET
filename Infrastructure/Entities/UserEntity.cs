using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Entities;

public class UserEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;
    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;
    public int? AddressId { get; set; }
    public string? Biography { get; set; }
    public string? ImgUrl { get; set; }
    public AddressEntity? Address { get; set; }
    public bool IsExternalAccount { get; set; } = false;
}
