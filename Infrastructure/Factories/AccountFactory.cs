using Infrastructure.Entities;
using Infrastructure.Models.AccountModels;
using Infrastructure.Repositories;

namespace Infrastructure.Factories;

public class AccountFactory(AddressRepository addressRepository)
{
    private readonly AddressRepository _addressRepository = addressRepository;

    public AccountDetailsBasicInfoModel PopulateBasicInfo(UserEntity user)
    {
        var basicInfo = new AccountDetailsBasicInfoModel()
        {
            ProfileImage = user.ImgUrl,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Phone = user.PhoneNumber!,
            Biography = user.Biography,
            IsExternalAccount = user.IsExternalAccount,
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
}
