using Assigment.ModelsDao;

namespace Assigment.Models
{
    /// <summary>
    /// Represents a proposal related to an item.
    /// </summary>
    public class Proposal
    {
        /// <summary>
        /// Gets or sets the unique identifier of the proposal.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the item associated with this proposal.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the comment associated with this proposal.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the proposal was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the updating date of the proposal.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the payment information related to this proposal.
        /// </summary>
        public string Payment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of payment associated with this proposal (e.g., fixed, installment).
        /// </summary>
        public string PaymentType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the item associated with this proposal.
        /// </summary>
        public ItemDao Item { get; set; }

        /// <summary>
        /// Gets or sets the list of involved parties in this proposal.
        /// </summary>
        public List<InvolvedParties> InvolvedParties { get; set; } = [];

        /// <summary>
        /// Gets or sets the unique identifier of the user who created the proposal.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who created the proposal.
        /// </summary>
        public UserModel CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who created the proposal.
        /// </summary>
        public string CreatedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of counter-proposals associated with this proposal.
        /// </summary>
        public List<CounterProposal> CounterProposals { get; } = [];
    }

}
