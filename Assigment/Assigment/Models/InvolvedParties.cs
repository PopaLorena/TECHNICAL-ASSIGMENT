using Assigment.ModelsDao;

namespace Assigment.Models
{
    /// <summary>
    /// Represents an involved party in a proposal negotiation.
    /// </summary>
    public class InvolvedParties
    {
        /// <summary>
        /// Gets or sets the unique identifier for the involved party record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the acceptance status of the involved party.
        /// Null = pending, true = accepted, false = rejected.
        /// </summary>
        public bool? IsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the party involved in the proposal.
        /// </summary>
        public Guid PartyId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the proposal that the party is involved in.
        /// </summary>
        public Guid ProposalId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who accepted or rejected the involved party.
        /// </summary>
        public Guid? AcceptedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who accepted or rejected the involved party.
        /// </summary>
        public string AcceptedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user who accepted or rejected the involved party.
        /// </summary>
        public UserModel? AcceptedByUser { get; set; } = null;
    }

}
