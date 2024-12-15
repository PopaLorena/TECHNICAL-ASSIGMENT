namespace Assigment.Models
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password of the user (plaintext). Should be hashed and salted before storage.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hashed password of the user.
        /// </summary>
        public byte[]? PasswordHash { get; set; } = null;

        /// <summary>
        /// Gets or sets the salt used in the password hashing process.
        /// </summary>
        public byte[]? PasswordSalt { get; set; } = null;

        /// <summary>
        /// Gets or sets the unique identifier of the party associated with this user.
        /// </summary>
        public Guid PartyId { get; set; }

        /// <summary>
        /// Gets or sets the party associated with this user.
        /// </summary>
        public Party? Party { get; set; }
    }
}
