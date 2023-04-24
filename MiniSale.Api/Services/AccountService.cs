using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniSale.Api.Application.Commands.Account;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Models.Account.Response;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenGenerationService _tokenGenerationService;

        public AccountService(UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              SignInManager<ApplicationUser> signInManager,
                              ITokenGenerationService tokenGenerationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenGenerationService = tokenGenerationService ?? throw new ArgumentNullException(nameof(tokenGenerationService));
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<List<IdentityRole>> GetSystemRolsAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<(SignInResult SignInResult, AccessTokenResponseModel TokenResponse)> LoginByUserName(LoginCommand model, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberLogin, lockoutOnFailure: false);
            if (!result.Succeeded)
                return (result, null);

            var token = await _tokenGenerationService.GenerateAccessTokenAsync(user, cancellationToken: cancellationToken);
            return (result, token);
        }

        public async Task<bool> IsSameUserNameExist(string userName, CancellationToken cancellationToken)
        {
            userName = userName.Trim().ToLower();
            var res = await _userManager.Users.AnyAsync(x => x.UserName.ToLower() == userName);
            return res;
        }

        public async Task<bool> IsSameEmailExist(string email, CancellationToken cancellationToken)
        {
            email = email.Trim().ToLower();
            var res = await _userManager.Users.AnyAsync(x => x.Email.ToLower() == email);
            return res;
        }
    }
}
