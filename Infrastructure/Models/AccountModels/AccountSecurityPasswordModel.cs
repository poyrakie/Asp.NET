using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.AccountModels;

public class AccountSecurityPasswordModel
{
    [DataType(DataType.Password)]
    [Display(Name = "Current password", Prompt = "Enter your old password", Order = 0)]
    [Required(ErrorMessage = "You must enter your current password")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Invalid password")]
    public string CurrentPassword { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "New password", Prompt = "Enter your new password", Order = 1)]
    [Required(ErrorMessage = "You must enter your new password")]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^\\da-zA-Z]).{8,}$", ErrorMessage = "Invalid password")]
    public string NewPassword { get; set; } = null!;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password", Prompt = "Confirm your new password", Order = 2)]
    [Required(ErrorMessage = "You must confirm your new password")]
    [Compare(nameof(NewPassword), ErrorMessage = "Passwords does not match")]
    public string ConfirmPassword { get; set; } = null!;

}
