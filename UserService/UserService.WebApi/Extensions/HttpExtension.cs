using System.Security.Claims;
using UserService.Core.Exceptions;

namespace UserService.WebApi.Extensions
{
    public static class HttpExtension
    {
        public static string GetUserLogin(this HttpContext context)
        {
            Claim? claim = context.User.FindFirst("Login") ?? throw new BadRequestException("Check your token");
            return claim.Value;
        }

        public static Guid GetUserId(this HttpContext context)
        {
            Claim? claim = context.User.FindFirst("Id") ?? throw new BadRequestException("Check your token");
            if (!Guid.TryParse(claim.Value, out Guid id))
                throw new BadRequestException("Invalid field in token");
            return id;
        }
    }
}