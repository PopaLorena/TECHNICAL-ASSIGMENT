namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for a proposal.
    /// </summary>
    public class ProposalDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the proposal.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the proposal.
        /// </summary>
        public string CreatedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comment associated with the proposal.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the proposal.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updating date of the proposal.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the payment details associated with the proposal.
        /// </summary>
        public string Payment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of involved parties associated with the proposal.
        /// </summary>
        public List<InvolvedPartiesDto> InvolvedParties { get; set; } = [];

        /// <summary>
        /// Gets the list of counter proposals associated with the proposal.
        /// </summary>
        public List<CounterProposalDto> CounterProposals { get; } = [];

    }
}
