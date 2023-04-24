using MediatR;
using Microsoft.AspNetCore.Http;
using MiniSale.Api.Infrastructure.BaseReuqestTypes;
using MiniSale.Api.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSale.Api.Application.Behaviors
{
    public class AuthenticationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : BaseRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public AuthenticationBehavior(IHttpContextAccessor httpContextAccessor, IIdentityService identityService)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var userId = _identityService.GetIdentity();
            request.SetRequestId(Guid.NewGuid());
            request.SetUserId(userId);
            return await next();
        }
    }
}
