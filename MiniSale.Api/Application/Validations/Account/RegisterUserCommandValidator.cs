using FluentValidation;
using MiniSale.Api.Application.Commands.Account;
using MiniSale.Api.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using MiniSale.Api.Application.Authentication;

namespace MiniSale.Api.Application.Validations.Account
{
    public class RegisterUserCommandValidator : CRUDCommandValidator<RegisterUserCommand>
    {
        private readonly IAccountService _accountService;

        public RegisterUserCommandValidator(IAccountService accountService) : base()
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

            RuleFor(x => x.UserName)
               .NotNull()
               .NotEmpty()
               .MaximumLength(100);

            RuleFor(x => x.UserName)
                .MustAsync(CheckUserNameExists)
                .When(x => !string.IsNullOrWhiteSpace(x.UserName))
                .WithMessage(c => $"An user with the user name {c.UserName} already exists.");

            RuleFor(c => c.UserPass)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.Email)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .MustAsync(CheckEmailExists)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage(c => $"An user with the email {c.Email} already exists.");

            RuleFor(c => c.FirstName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.LastName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(c => c.Roles)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Roles)
                .Must(RolesExists)
                .When(x => x.Roles != null && x.Roles.Any())
                .WithMessage(c => $"An roles with name {string.Join(",", c.Roles)} do not exists.");
        }

        async Task<bool> CheckUserNameExists(string userName, CancellationToken cancellationToken)
        {
            var res = await _accountService.IsSameUserNameExist(userName, cancellationToken: cancellationToken);
            return !res;
        }

        async Task<bool> CheckEmailExists(string email, CancellationToken cancellationToken)
        {
            var res = await _accountService.IsSameEmailExist(email, cancellationToken: cancellationToken);
            return !res;
        }

        bool RolesExists(string[] roles)
        {
            return roles.Any(x => !Roles.RoleNames.Contains(x.Trim().ToLower()));
        }
    }
}
