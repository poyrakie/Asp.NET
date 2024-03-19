using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Models.AuthModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services;

public class UserService(UserRepository repo, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, UserFactory userFactory, UserRepository userRepository)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly UserRepository _repo = repo;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserRepository _userRepository = userRepository;

    public async Task<ResponseResult> RegisterUserAsync(SignUpFormModel form)
    {
        try
        {
            var result = await CheckIfUserExistsAsync(form.Email);
            if (result.StatusCode == StatusCode.NOT_FOUND)
            {
                var factoryResult = _userFactory.PopulateUserEntity(form);
                if (factoryResult.StatusCode == StatusCode.OK)
                {
                    var userManagerResult = await _userManager.CreateAsync((UserEntity)factoryResult.ContentResult!, form.Password);
                    if (userManagerResult.Succeeded)
                        return ResponseFactory.Ok("User registered successfully");
                }
            }
            else
            {
                return result;
            }
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> CheckIfUserExistsAsync(string email)
    {
        try
        {
            var result = await _repo.ExistsAsync(x => x.Email == email);
            return result;

        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }

    public async Task<ResponseResult> SignInUserAsync(SignInFormModel form)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, false);
            if (result.Succeeded)
                return ResponseFactory.Ok();
            return ResponseFactory.Error("Incorrect email or password.");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> FindUserAsync(string email)
    {
        try
        {
            var checkResult = await CheckIfUserExistsAsync(email);
            if (checkResult.StatusCode == StatusCode.EXISTS)
            {
                var result = await _userManager.FindByEmailAsync(email);
                if (result != null)
                {
                    return ResponseFactory.Exists(result);
                }
            }
            return ResponseFactory.NotFound();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> RegisterOrUpdateExternalAsync(ExternalLoginInfo info)
    {
        try
        {
            var populateResult = _userFactory.PopulateUserEntity(info);
            var userEntity = (UserEntity)populateResult.ContentResult!;
            var findResult = await FindUserAsync(userEntity.Email!);
            var user = (UserEntity)findResult.ContentResult!;
            if (findResult.StatusCode == StatusCode.NOT_FOUND)
            {

                var result = await _userManager.CreateAsync(userEntity);
                if (result.Succeeded)
                {
                    userEntity = await _userManager.FindByEmailAsync(userEntity.Email!);
                    return ResponseFactory.Ok(userEntity!, "New user registered");
                }
                return ResponseFactory.Error("Something went wrong, could not register user");
            }

            else if (findResult.StatusCode == StatusCode.EXISTS)
            {

                if (user.FirstName != userEntity.FirstName || user.LastName != userEntity.LastName || user.Email != userEntity.Email)
                {
                    var rePopulateResult = _userFactory.PopulateUserEntity(user, userEntity);
                    user = (UserEntity)rePopulateResult.ContentResult!;
                    await _userManager.UpdateAsync(user);
                }
                return ResponseFactory.Ok(user, "Updated user");
            }
            else
            {
                return ResponseFactory.Error("Something went wrong");
            }
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

    public async Task<ResponseResult> UpdateBasicInfoAsync(AccountDetailsBasicInfoModel model, UserEntity user)
    {
        try
        {
            if (model.Email != user.Email)
            {
                var existsResult = await _userRepository.ExistsAsync(x => x.Email == model.Email);
                if (existsResult.StatusCode == StatusCode.EXISTS)
                {
                    return ResponseFactory.Exists("A user with this email is already registered");
                }
            }

            var responseResult = _userFactory.PopulateUserEntity(model, user!);
            var entity = (UserEntity)responseResult.ContentResult!;

            var result = await _userManager.UpdateAsync(entity!);
            if (result.Errors.Any())
            {
                return ResponseFactory.Error("Something went wrong");
            }
            return ResponseFactory.Ok("Updated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> UpdateBasicInfoAsync(UserEntity user)
    {
        try
        {
            var result = await _userManager.UpdateAsync(user!);
            if (result.Errors.Any())
            {
                return ResponseFactory.Error("Something went wrong");
            }
            return ResponseFactory.Ok("Updated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> UpdateExternalAsync(AccountDetailsBasicInfoModel model, ExternalLoginInfo externalUser, ClaimsIdentity claims)
    {
        try
        {
            if (claims?.Name is not null)
            {
                if (externalUser.LoginProvider == "Facebook" || externalUser.LoginProvider == "Google")
                {
                    var existingUser = await _userManager.FindByEmailAsync(claims.Name);

                    if (existingUser is not null)
                    {
                        existingUser.Biography = model.Biography;
                        existingUser.PhoneNumber = model.Phone;

                        var result = await UpdateBasicInfoAsync(existingUser);

                        if (result.StatusCode == StatusCode.OK)
                        {
                            return ResponseFactory.Ok("Account information successfully updated");
                        }
                    }
                }
            }
            return ResponseFactory.Error("Something went wrong.");
        }
        catch(Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
}
