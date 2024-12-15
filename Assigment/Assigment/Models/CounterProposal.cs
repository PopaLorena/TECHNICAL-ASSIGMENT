namespace Assigment.Models
{
    /// <summary>
    /// Represents a counter proposal in the system.
    /// </summary>
    public class CounterProposal
    {
        /// <summary>
        /// Gets or sets the unique identifier for the counter proposal.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the proposal that this counter proposal is responding to.
        /// </summary>
        public Guid ProposalId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the counter proposal was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who created the counter proposal.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who created the counter proposal.
        /// </summary>
        public UserModel? CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the payment details for the counter proposal.
        /// </summary>
        public string Payment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of payment for the counter proposal.
        /// </summary>
        public string PaymentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the user who created the counter proposal.
        /// </summary>
        public string CreatedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets any additional comments for the counter proposal.
        /// </summary>
        public string Comment { get; set; }
    }

}
