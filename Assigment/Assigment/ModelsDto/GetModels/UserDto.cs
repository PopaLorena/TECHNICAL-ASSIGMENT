namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for a user.
    /// </summary>
    public class UserDto
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
        /// Gets or sets the unique identifier of the party the user is associated with.
        /// </summary>
        public Guid PartyId { get; set; }
    }
}
