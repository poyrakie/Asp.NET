using Infrastructure.Entities;
using Infrastructure.Models.AccountModels;

namespace Infrastructure.Factories;

public class AccountFactory()
{

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
}
