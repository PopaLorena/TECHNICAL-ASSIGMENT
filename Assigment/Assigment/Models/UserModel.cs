namespace Assigment.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; } = null;
        public byte[]? PasswordSalt { get; set; } = null;
        public Guid PartyId { get; set; } 

    }
}
