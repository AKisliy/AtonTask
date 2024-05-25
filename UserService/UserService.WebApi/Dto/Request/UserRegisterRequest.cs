using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserService.WebApi.Dto.Request
{
    public class UserRegisterRequest
    {
        [Required, RegularExpression("^[0-9A-Za-z]*$", ErrorMessage = "Login should contain only numbers and/or latin letters")]
        [DefaultValue("login")]
        public string Login { get; set; } = null!;

        [Required, RegularExpression("^[0-9A-Za-z]*$", ErrorMessage = "Password should contain only numbers and/or latin letters")]
        [DefaultValue("password")]
        public string Password { get; set; } = null!;

        [Required, RegularExpression("^[A-Za-zА-Яа-я]*$", ErrorMessage = "Name should contain only Russian and/or Latin letters")]
        [DefaultValue("name")]
        public string Name { get; set; } = null!;

        [Required, Range(0,2)]
        public int Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool IsAdmin { get; set; }
    }
}