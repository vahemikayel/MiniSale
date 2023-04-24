using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniSale.Api.Infrastructure.Options;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace MiniSale.Api.Application.Authentication
{
    internal static class AuthenticationServiceCollectionExtensions
    {
        internal static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services,
                                                                    List<string> hubUrls = null)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            var urlsOption = services.BuildServiceProvider().GetRequiredService<IOptions<UriOptions>>().Value;
            //var identityUrl = new Uri(new Uri(urlsOption.IntegrationUri), "api").AbsoluteUri;

            var identityConf = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityJWTOptions>>().Value;
            var key = Encoding.ASCII.GetBytes(identityConf.JWT.Secret);
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidIssuer = urlsOption.BaseUri,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddSingleton(tokenValidationParams);

            var authBuilder = services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            
            authBuilder.AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.ClaimsIssuer = urlsOption.BaseUri;
                options.TokenValidationParameters = tokenValidationParams;
                options.Events = new JwtBearerEvents()
                {
                    OnForbidden = frb =>
                    {
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = ctx =>
                    {
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            if (context.Principal.Identity is ClaimsIdentity identity)
                            {
                                identity.AddClaim(new Claim("access_token", accessToken.RawData));
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = mr =>
                    {
                        var path = mr.HttpContext.Request.Path;

                        var accessToken = mr.Request.Headers["access_token"];
                        if (string.IsNullOrEmpty(accessToken))
                            accessToken = mr.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            mr.Token = accessToken;
                            if (hubUrls != null && hubUrls.Count > 0 && hubUrls.Any(x => "/" + x == path.Value))
                            {
                                return Task.CompletedTask;
                            }
                            mr.HttpContext.Request.Headers.TryAdd("Authorization", "Bearer " + accessToken);
                        }

                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            authBuilder.AddCookie(options =>
            {
                options.Cookie.IsEssential = true;
            });

            return authBuilder;
        }
    }
}
