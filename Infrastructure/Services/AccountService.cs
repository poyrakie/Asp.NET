using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Models;
using Infrastructure.Models.AccountModels;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Services;

public class AccountService(SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager, UserRepository userRepository, AccountFactory accountFactory)
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly UserRepository _userRepository = userRepository;
    private readonly AccountFactory _accountFactory = accountFactory;



}
