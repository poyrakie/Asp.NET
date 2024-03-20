using Infrastructure.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.AccountModels;

public class AccountSecurityDeleteModel
{
    [Display(Name = "Yes, I want to delete my account.", Order = 0)]
    [CheckBoxRequired(ErrorMessage = "You must confirm that you want to delete your account.")]
    public bool DeleteConfirmation { get; set; } = false;
}
