namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for an involved party in a proposal or process.
    /// </summary>
    public class InvolvedPartiesDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the involved party.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the acceptance status of the involved party.
        /// Null means pending, true means accepted, and false means rejected.
        /// </summary>
        public bool? IsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who accepted or rejected the party's involvement.
        /// </summary>
        public string AcceptedByUserName { get; set; } = string.Empty; 
    }
}
