using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating a new user.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <example>John</example>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        /// <example>johndoe@example.com</example>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password for the user account.
        /// </summary
        /// <example>P@ssw0rd123!</example>
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmation password for the user account.
        /// Must match the Password property.
        /// </summary>
        /// <example>P@ssw0rd123!</example>
        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare(nameof(Password),ErrorMessage = "ConfirmPassword does not match the Password")]
        public required string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the party associated with the user.
        /// </summary>
        [Required(ErrorMessage = "Party is required")]
        public Guid PartyId { get; set; }
    }
}
