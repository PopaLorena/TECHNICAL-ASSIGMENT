using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [Compare(nameof(Password),ErrorMessage = "ConfirmPassword does not match the Password")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Party is required")]
        public Guid PartyId { get; set; }
    }
}
