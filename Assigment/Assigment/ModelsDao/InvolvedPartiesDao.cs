using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents an involved party in a proposal.
    /// </summary>
    public class InvolvedPartiesDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the involved party record.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the status of the involved party's response to the proposal.
        /// Null = pending, true = accepted, false = rejected.
        /// </summary>
        public bool? IsAccepted { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated party.
        /// </summary>
        public Guid? PartyId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated proposal.
        /// </summary>
        public Guid? ProposalId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the user who accepted or rejected the proposal.
        /// </summary>
        public Guid? AcceptedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the user who accepted or rejected the proposal.
        /// </summary>
        public UserDao? AcceptedByUser { get; set; } = null;

        /// <summary>
        /// Gets or sets the associated proposal for the involved party.
        /// </summary>
        public ProposalDao? Proposal { get; set; } = null;
    }

}
