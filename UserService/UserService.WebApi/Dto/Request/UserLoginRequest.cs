using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.Dto.Request
{
    public class UserLoginRequest
    {
        [Required, RegularExpression("^[0-9A-Za-z]*$", ErrorMessage = "Login should contain only numbers and/or latin letters")]
        [DefaultValue("login")]
        public string Login { get; set; } = null!;
        [Required, RegularExpression("^[0-9A-Za-z]*$", ErrorMessage = "Password should contain only numbers and/or latin letters")]
        [DefaultValue("password")]
        public string Password { get; set; } = null!;
    }
}