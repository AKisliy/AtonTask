using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UserService.Application.Services;
using UserService.Core.Interfaces;
using UserService.Core.Interfaces.Infrastructure;
using UserService.Core.Interfaces.Services;
using UserService.DataAccess.Repositories;
using UserService.Infrastructure;
using UserService.Infrastructure.Options;

namespace UserService.WebApi.Extensions
{
    public static class ApiExtension
    {
        public static void AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
            var cookies = new MyCookiesOptions();
            configuration.GetSection(nameof(MyCookiesOptions)).Bind(cookies);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddCookie("cookie")
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
                {
                    options.TokenValidationParameters = new() 
                    {
                        ValidateIssuer = jwtOptions.ValidateIssuer,
                        ValidateAudience = jwtOptions.ValidateAudience,
                        ValidateLifetime = jwtOptions.ValidateLifetime,
                        ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        //ClockSkew = new TimeSpan(0,0,5) uncomment for testing purposes
                    };
                    // get token from cookies
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[cookies.TokenFieldName];

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorizationBuilder()
                .AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin", "True"));
        }

        public static void ConfigureCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.Configure<MyCookiesOptions>(configuration.GetSection(nameof(MyCookiesOptions)));
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UsersService>();
        }

        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IValidationHelper, ValidationHelper>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}