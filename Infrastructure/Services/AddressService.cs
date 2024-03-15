using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class AddressService(AddressRepository repo, AddressFactory addressFactory, UserManager<UserEntity> userManager)
{
    private readonly AddressRepository _repo = repo;
    private readonly AddressFactory _addressFactory = addressFactory;
    private readonly UserManager<UserEntity> _userManager = userManager;


    public async Task<ResponseResult> GetOrCreateAddressAsync(AccountDetailsAddressInfoModel form)
    {
        try
        {
            Expression<Func<AddressEntity, bool>> addressExpression = x => x.AddressLine_1 == form.Addressline_1 && x.AddressLine_2 == form.Addressline_2 && x.PostalCode == form.PostalCode && x.City == form.City;
            var result = await _repo.ExistsAsync(addressExpression);
            if (result.StatusCode == StatusCode.EXISTS)
            {
                var getResult = await _repo.GetAsync(addressExpression);
                if (getResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok(getResult.ContentResult!);
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
            {

                var addressEntity = _addressFactory.PopulateAddressEntity(form);
                var createResult = await _repo.CreateAsync(addressEntity);
                if (createResult.StatusCode == StatusCode.OK)
                    return ResponseFactory.Ok(createResult.ContentResult!);
            }
            return ResponseFactory.Error("Something went wrong");
        }
        catch (Exception ex) { return ResponseFactory.Error(ex.Message); }
    }
    public async Task<ResponseResult> UpdateAddressInfoAsync(AccountDetailsAddressInfoModel model, UserEntity user)
    {
        try
        {
            var addressResult = await GetOrCreateAddressAsync(model);
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
