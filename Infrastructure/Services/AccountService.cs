using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
namespace Infrastructure.Services;

public class AccountService(UserManager<UserEntity> userManager, IConfiguration configuration)
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<ResponseResult> UploadProfileImageAsync(UserEntity user, IFormFile file)
    {
        try
        {
            if (file != null && file.Length != 0)
            {
                var fileName = $"p_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), _configuration["FileUploadPath"]!, fileName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(fs);

                user.ImgUrl = fileName;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return ResponseFactory.Ok();
                }
            }
            return ResponseFactory.Error();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }

}
