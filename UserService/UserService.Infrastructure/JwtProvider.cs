using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Core.Interfaces.Infrastructure;
using UserService.Core.Models;
using UserService.Infrastructure.Options;

namespace UserService.Infrastructure
{
    public class JwtProvider(IOptions<JwtOptions> options): IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateToken(User user)
        {
            Claim[] claims = [new("Id", user.Id.ToString()), new("Login", user.Login),new("IsAdmin", user.IsAdmin.ToString())];
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes),
                signingCredentials: signingCredentials);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenValue;
        }
    }
}