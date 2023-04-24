using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace MiniSale.Api.Infrastructure.AutoMapperExtensions
{
    internal static class AutoMapperExtension
    {
        internal static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var configuration = new MapperConfiguration(mc =>
            {
                mc.AddProfile<AccountProfile>();
            });

            services.AddSingleton(sp => configuration.CreateMapper());
            return services;
        }
    }
}
