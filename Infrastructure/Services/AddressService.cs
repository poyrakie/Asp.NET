using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class AddressService(AddressRepository repo, AddressFactory addressFactory)
{
    private readonly AddressRepository _repo = repo;
    private readonly AddressFactory _addressFactory = addressFactory;


    public async Task<ResponseResult> GetOrCreateAddressAsync(AccountDetailsAddressInfoModel form)
    {
        try
        {
            Expression<Func<AddressEntity, bool>> addressExpression = x => x.AddressLine_1 == form.Addressline_1 && x.AddressLine_2 == form.Addressline_2 && x.PostalCode == form.PostalCode && x.City == form.City;
            var result = await _repo.ExistsAsync(addressExpression);
            if (result.StatusCode == StatusCode.EXISTS)
            {
                var getResult = await _repo.GetAsync(addressExpression);
                if(getResult.StatusCode == StatusCode.OK)
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
}
