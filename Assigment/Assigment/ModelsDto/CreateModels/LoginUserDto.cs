using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for user login.
    /// </summary>
    public class LoginUserDto
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log in.
        /// </summary>
        /// <example>user@example.com</example>
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// </summary>
        /// <example>P@ssw0rd!</example>
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
