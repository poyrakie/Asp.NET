using Infrastructure.Entities;
using Infrastructure.Models.AccountModels;

namespace Infrastructure.Factories;

public class AddressFactory
{
    public AddressEntity PopulateAddressEntity(AccountDetailsAddressInfoModel form)
    {
        var addressEntity = new AddressEntity
        {
            AddressLine_1 = form.Addressline_1,
            AddressLine_2 = form.Addressline_2,
            PostalCode = form.PostalCode,
            City = form.City,
        };
        return addressEntity;
    }
}
