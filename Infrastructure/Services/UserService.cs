using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Models.AuthModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserRepository repo, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, UserFactory userFactory)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly UserRepository _repo = repo;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;

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
}
