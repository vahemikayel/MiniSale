using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Models.Account.Response;
using MiniSale.Api.Services;
using MiniSale.Generic.Repository.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Application.Commands.Account
{
    public class LoginCommand : BaseHttpRequest<LoginResponseModel>
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseModel>
    {
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginCommandHandler(ITokenGenerationService tokenGenerationService,
                                   SignInManager<ApplicationUser> signInManager,
                                   UserManager<ApplicationUser> userManager,
                                   IUnitOfWork unitOfWork)
        {
            _tokenGenerationService = tokenGenerationService ?? throw new ArgumentNullException(nameof(tokenGenerationService));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager;
        }

        public async Task<LoginResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, request.RememberLogin, lockoutOnFailure: false);
            if (!result.Succeeded)
                return new LoginResponseModel(result);

            var token = await _tokenGenerationService.GenerateAccessTokenAsync(user, cancellationToken: cancellationToken);
            return new LoginResponseModel(result, token);
        }
    }
}
