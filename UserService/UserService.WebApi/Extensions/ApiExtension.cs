using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin", "True"));
            });
        }
    }
}