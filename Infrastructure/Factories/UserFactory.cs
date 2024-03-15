using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Models.AuthModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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
    public ResponseResult PopulateUserEntity(AccountDetailsBasicInfoModel model, UserEntity entity)
    {
        try
        {
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.PhoneNumber = model.Phone;
            entity.Biography = model.Biography;

            return ResponseFactory.Ok(entity, "Populated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public ResponseResult PopulateUserEntity(ExternalLoginInfo info)
    {
        try
        {
            var result = new UserEntity
            {
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!,
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!,
                Email = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                UserName = info.Principal.FindFirstValue(ClaimTypes.Email)!,
                IsExternalAccount = true
            };
            return ResponseFactory.Ok(result, "Populated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public ResponseResult PopulateUserEntity(UserEntity user, UserEntity userEntity)
    {
        try
        {
            user.FirstName = userEntity.FirstName;
            user.LastName = userEntity.LastName;
            user.Email = userEntity.Email;

            return ResponseFactory.Ok(user, "Populated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
