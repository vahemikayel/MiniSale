using Microsoft.Extensions.DependencyInjection;

namespace MiniSale.Api.Application.Authentication
{
    internal static class AuthorizationServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Policies.Admin, policy =>
                {
                    policy.RequireClaim("role", Roles.Admin);
                });

                options.AddPolicy(Policies.Management, policy =>
                {
                    policy.RequireClaim("role", Roles.Manage);
                });

                options.AddPolicy(Policies.Operator, policy =>
                {
                    policy.RequireClaim("role", Roles.Operator, Policies.Admin);
                });
            });
            return services;
        }
    }
}
