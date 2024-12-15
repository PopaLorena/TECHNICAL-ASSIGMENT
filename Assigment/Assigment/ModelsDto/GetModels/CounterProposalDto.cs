namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for a counter proposal.
    /// </summary>
    public class CounterProposalDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the counter proposal.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated proposal.
        /// </summary>
        public Guid ProposalId { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the counter proposal.
        /// </summary>
        public string CreatedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comment associated with the counter proposal.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the payment details for the counter proposal.
        /// </summary>
        public string Payment { get; set; } = string.Empty;
    }
}
