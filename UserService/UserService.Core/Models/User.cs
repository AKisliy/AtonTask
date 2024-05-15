namespace UserService.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime? ModifiedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? RevokedOn { get; set; }

        public string? RevokeBy { get; set; }

        public bool IsAdmin { get; set; }
    }
}