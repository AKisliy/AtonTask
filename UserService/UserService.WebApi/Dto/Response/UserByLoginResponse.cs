namespace UserService.WebApi.Dto.Response
{
    public class UserByLoginResponse
    {
        public string Name { get; set; } = null!;

        public int Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool IsActive { get; set; }
    }
}