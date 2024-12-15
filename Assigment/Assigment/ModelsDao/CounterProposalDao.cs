using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents a counter proposal in the system, which is associated with a proposal and created by a user.
    /// </summary>
    public class CounterProposalDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the counter proposal.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated proposal.
        /// </summary>
        public Guid ProposalId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the user who created the counter proposal.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the payment details for the counter proposal.
        /// </summary>
        public string Payment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the comment associated with the counter proposal.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the counter proposal was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the user who created the counter proposal.
        /// </summary>
        public UserDao? CreatedByUser { get; set; }
    }
}
