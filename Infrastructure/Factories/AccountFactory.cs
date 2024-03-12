using Infrastructure.Entities;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Infrastructure.Factories;

public class AccountFactory(UserFactory userFactory, UserManager<UserEntity> userManager, AddressService addressService, AddressRepository addressRepository, UserRepository userRepository)
{
    private readonly UserFactory _userFactory = userFactory;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly AddressService _addressService = addressService;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly UserRepository _userRepository = userRepository;

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
    public async Task<AccountDetailsAddressInfoModel> PopulateAddressInfoAsync(UserEntity user)
    {
        var result = await _addressRepository.GetAsync(x => x.Id == user.AddressId);
        var addressEntity = (AddressEntity)result.ContentResult!;
        var addressInfo = new AccountDetailsAddressInfoModel();
        if (user.Address != null)
        {

            addressInfo.Addressline_1 = addressEntity.AddressLine_1;
            addressInfo.Addressline_2 = addressEntity.AddressLine_2;
            addressInfo.PostalCode = addressEntity.PostalCode;
            addressInfo.City = addressEntity.City;
        }
        return addressInfo;
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
                    return ResponseFactory.Error("Something went wrong");
                }
            }
            return ResponseFactory.Ok("Updated successfully");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }

    }
}
