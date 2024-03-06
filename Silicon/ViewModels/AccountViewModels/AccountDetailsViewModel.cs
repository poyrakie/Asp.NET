using Infrastructure.Models.AccountModels;

namespace Silicon.ViewModels.AccountViewModels;

public class AccountDetailsViewModel
{
    public string Title { get; set; } = "Account Details";
    public AccountDetailsBasicInfoModel BasicInfo { get; set; } = new AccountDetailsBasicInfoModel() 
    {
        ProfileImage = "images/profile-image.svg",
        FirstName = "Johan",
        LastName = "Åström",
        Email = "johan@domain.com",
        Phone = "072345678"
    };
    public AccountDetailsAddressInfoModel AddressInfo { get; set; } = new AccountDetailsAddressInfoModel();
}
