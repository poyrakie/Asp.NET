using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Factories;

public class AccountFactory(UserFactory userFactory, UserManager<UserEntity> userManager, AddressService addressService)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AddressService _addressService = addressService;

    public AccountDetailsBasicInfoModel PopulateBasicInfo(UserEntity user)
    {
        var basicInfo = new AccountDetailsBasicInfoModel()
        {
            ProfileImage = user.ImgUrl,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Phone = user.PhoneNumber!,
            Biography = user.Biography
        };
        return basicInfo;
    }
    public AccountDetailsAddressInfoModel PopulateAddressInfo(UserEntity user)
    {
        var addressInfo = new AccountDetailsAddressInfoModel();
        if (user.Address != null)
        {
            addressInfo.Addressline_1 = user.Address!.AddressLine_1;
            addressInfo.Addressline_2 = user.Address.AddressLine_2;
            addressInfo.PostalCode = user.Address.PostalCode;
            addressInfo.City = user.Address.City;
        }
        return addressInfo;
    }
    public async Task<ResponseResult> UpdateBasicInfoAsync(AccountDetailsBasicInfoModel model, UserEntity user)
    {
        try
        {
            var responseResult = _userFactory.PopulateUserEntity(model, user!);
            var entity = (UserEntity)responseResult.ContentResult!;
            var result = await _userManager.UpdateAsync(entity!);
            if (result.Errors.Any())
            {
                return ResponseFactory.Error();
            }
            return ResponseFactory.Ok();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> UpdateAddressInfoAsync(AccountDetailsAddressInfoModel model, UserEntity user)
    {
        try
        {
            var addressResult = await _addressService.GetOrCreateAddressAsync(model);
            if (addressResult.StatusCode == StatusCode.OK)
            {
                var addressEntity = (AddressEntity)addressResult.ContentResult!;
                user!.AddressId = addressEntity.Id;
                var userResult = await _userManager.UpdateAsync(user);
                if (userResult.Errors.Any())
                {
                    return ResponseFactory.Error();
                }
            }
            return ResponseFactory.Ok();
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }

    }
}
