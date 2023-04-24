using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MiniSale.Api.Application.Authentication;
using MiniSale.Api.Application.Commands.Account;
using MiniSale.Api.Infrastructure.Options;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSale.Api.Controllers.V1
{
    [Authorize(Policy = Policies.Admin)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{api-version:apiVersion}/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IOptions<UriOptions> _options;
        private readonly IAccountService _accountService;

        public AccountController(IMediator mediator, SignInManager<ApplicationUser> signInManager, IOptions<UriOptions> options, IAccountService accountService)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<object>> Login([FromBody] LoginCommand model, CancellationToken cancellationToken = default)
        {
            //var result = await _mediator.Send(command, cancellationToken);
            //if (!result)
            //    return BadRequest("Incorrect credentials.");

            //return Ok(result);

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }

                if (ModelState.IsValid)
                {
                    var result = await _accountService.LoginByUserName(model, cancellationToken);
                    if (result.SignInResult.Succeeded)
                    {
                        var responseJson = JsonConvert.SerializeObject(result.TokenResponse);
                        Response.Headers.Add("access_token", responseJson);

                        Url.ActionContext.RouteData.DataTokens.Add("access_token", result.TokenResponse);
                        return new OkObjectResult(new { Token = result.TokenResponse.Token });
                    }

                    if (result.SignInResult.RequiresTwoFactor)
                    {
                        throw new NotImplementedException();
                    }
                    if (result.SignInResult.IsLockedOut)
                    {
                        throw new NotImplementedException("Lockout not implemented");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        throw new NotImplementedException("Invalid login UI does not implemented");
                    }
                }

                throw new NotImplementedException("BuildLogin not implemented");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterUserCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (!result)
                return BadRequest("User has not been created.");

            return Ok(result);
        }
    }
}
