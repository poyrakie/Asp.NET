using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.AccountModels;

public class AccountDetailsAddressInfoModel
{
    [Display(Name = "Address line 1", Prompt = "Enter your address line", Order = 0)]
    [Required(ErrorMessage = "Address line is required")]
    [MinLength(2, ErrorMessage = "Required")]
    public string Addressline_1 { get; set; } = null!;

    [Display(Name = "Address line 2", Prompt = "Enter your second last name", Order = 1)]
    public string? Addressline_2 { get; set; } 

    [Display(Name = "Postal code", Prompt = "Enter your postal code", Order = 2)]
    [Required(ErrorMessage = "Postal code is required")]
    [MinLength(2, ErrorMessage = "Required")]
    [DataType(DataType.PostalCode)]
    public string PostalCode { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter your City", Order = 3)]
    [Required(ErrorMessage = "City is required")]
    [MinLength(2, ErrorMessage = "Required")]
    public string City { get; set; } = null!;
}
