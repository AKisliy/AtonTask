namespace UserService.WebApi.Dto.Response
{
    public class UserResponse
    {
        public string Name { get; set; } = null!;

        public int Gender { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}