using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using MiniSale.Api.Application.Authentication;
using MiniSale.Api.Application.Behaviors;
using MiniSale.Api.Certificate;
using MiniSale.Api.Controllers;
using MiniSale.Api.Infrastructure.AutoMapperExtensions;
using MiniSale.Api.Infrastructure.Contexts.Identity;
using MiniSale.Api.Infrastructure.Contexts.Management;
using MiniSale.Api.Infrastructure.Filters;
using MiniSale.Api.Infrastructure.Middlewares;
using MiniSale.Api.Infrastructure.Options;
using MiniSale.Api.Models.Account.Entity;
using MiniSale.Api.Services;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Net;
using System.Reflection;

namespace MiniSale.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        private readonly bool _isDevelopmentOrStaging;
        internal IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _environment = env;
            _isDevelopmentOrStaging = env.IsDevelopment();

            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var mvcCoreBuilder = services.AddMvcCore()
                                         .SetCompatibilityVersion(CompatibilityVersion.Latest);
            ConfigureApiVersioning(services, mvcCoreBuilder);

            var mvcBuilder = services.AddMvc(option =>
            {
                //option.OutputFormatters.Insert(0, new PowerAppOutputFormater());

                option.EnableEndpointRouting = false;

                option.Filters.Add<CommandValidationExceptionFilter>();
            });

            mvcBuilder.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddAutoMapper();

            services.AddOptions();
            services.Configure<DBConnectionOptions>(Configuration.GetSection(DBConnectionOptions.SectionName));
            services.Configure<IdentityJWTOptions>(Configuration.GetSection(IdentityJWTOptions.SectionName));
            services.Configure<UriOptions>(Configuration.GetSection(UriOptions.SectionName));

            var dbConnections = services.BuildServiceProvider().GetRequiredService<IOptions<DBConnectionOptions>>().Value;
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(dbConnections.MainConnectionString,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(this.GetType().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         // sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(300), errorNumbersToAdd: null);
                                         sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", ApplicationDbContext.Schema);
                                     });
            }, ServiceLifetime.Scoped);
            
            services.AddDbContext<ManagementContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(dbConnections.MainConnectionString,
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(this.GetType().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         // sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(300), errorNumbersToAdd: null);
                                         sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", ManagementContext.Schema);
                                     });
            }, ServiceLifetime.Scoped);


            services.AddCors(o =>
            {
                o.AddPolicy("EnableCors", x => x.SetIsOriginAllowed(origin => true)
                                                .AllowAnyMethod()
                                                .AllowAnyHeader()
                                                .AllowCredentials());
            });

            ConfigureIdentity(services, dbConnections);

            //services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IIdentityService, IdentityService>();

            AddMediator(services);

        }

        private void ConfigureApiVersioning(IServiceCollection services, IMvcCoreBuilder mvcCoreBuilder)
        {
            mvcCoreBuilder.AddApiExplorer();
            services.AddVersionedApiExplorer();

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;

                o.Conventions.Controller<AccountController>();
                o.Conventions.Controller<InformerController>();
                o.Conventions.Controller<ReportController>();
            });
        }

        private void ConfigureIdentity(IServiceCollection services, DBConnectionOptions dbConnections)
        {
            var identityOptions = services.BuildServiceProvider().GetRequiredService<IOptions<IdentityJWTOptions>>();

            var cerManager = new CertificateManager(identityOptions);
            services.AddSingleton(cerManager);

            var identity = services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireDigit = identityOptions.Value.Password.RequireDigit;
                config.Password.RequireLowercase = identityOptions.Value.Password.RequireLowercase;
                config.Password.RequireUppercase = identityOptions.Value.Password.RequireUppercase;
                config.Password.RequireNonAlphanumeric = identityOptions.Value.Password.RequireNonAlphanumeric;
                config.Password.RequiredLength = identityOptions.Value.Password.RequiredLength;
                config.Lockout.DefaultLockoutTimeSpan = identityOptions.Value.Lockout.DefaultLockoutTimeSpan;
                config.Lockout.MaxFailedAccessAttempts = identityOptions.Value.Lockout.MaxFailedAccessAttempts;

            });
            identity.AddEntityFrameworkStores<ApplicationDbContext>();
            identity.AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.CheckConsentNeeded = context => true;
            });

            IdentityModelEventSource.ShowPII = true;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            | SecurityProtocolType.Tls11
            | SecurityProtocolType.Tls12;
            services.ConfigureAuthentication();

            services.ConfigureAuthorization();
            services.AddTransient<ITokenGenerationService, TokenGenerationService>();
        }

        private void AddMediator(IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthenticationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        }

        public void Configure(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var accContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                accContext.Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ManagementContext>();
                context.Database.Migrate();
            }

            app.UseMiddleware<ExceptionMiddleware>();
            app.InitializeDatabase();

            if (_isDevelopmentOrStaging)
            {
                //Configure swagger
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseSerilogRequestLogging();

            app.UseCors("EnableCors");
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapRazorPages();
            //    endpoints.MapDefaultControllerRoute();
            //    endpoints.MapControllers();
            //});
        }
        //var builder = WebApplication.CreateBuilder(args);

        //// Add services to the container.

        //builder.Services.AddControllers();
        //    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //    builder.Services.AddEndpointsApiExplorer();

        //    var app = builder.Build();

        //    // Configure the HTTP request pipeline.
        //    if (app.Environment.IsDevelopment())
        //    {

        //    }

        //    app.UseHttpsRedirection();

        //    app.UseAuthorization();

        //    app.MapControllers();

        //    app.Run();
    }
}
