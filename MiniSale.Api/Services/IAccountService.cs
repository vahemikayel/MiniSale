using Microsoft.AspNetCore.Identity;
using MiniSale.Api.Application.Commands.Account;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Models.Account.Response;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);

        Task<List<IdentityRole>> GetSystemRolsAsync();

        Task<(SignInResult SignInResult, AccessTokenResponseModel TokenResponse)> LoginByUserName(LoginCommand model, CancellationToken cancellationToken);

        Task<bool> IsSameUserNameExist(string userName, CancellationToken cancellationToken);
        Task<bool> IsSameEmailExist(string email, CancellationToken cancellationToken);
    }
}
