using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.AuthModels;

namespace Infrastructure.Factories;

public class UserFactory
{
    public ResponseResult PopulateUserEntity(SignUpFormModel model)
    {
        try
        {
            var result = new UserEntity
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };
            return ResponseFactory.Ok(result, "Populated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
