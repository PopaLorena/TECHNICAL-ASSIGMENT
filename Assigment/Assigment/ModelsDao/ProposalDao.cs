using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents a proposal in the system, associated with an item and created by a user. 
    /// It can have multiple involved parties and counter proposals.
    /// </summary>
    public class ProposalDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the proposal.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the item that this proposal is related to.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who created the proposal.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the comment associated with the proposal.
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the proposal was created.
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
        /// Gets or sets the user who created the proposal.
        /// </summary>
        public UserDao CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the item that the proposal is associated with.
        /// </summary>
        public ItemDao Item { get; set; }

        /// <summary>
        /// Gets the collection of involved parties in the proposal.
        /// </summary>
        public ICollection<InvolvedPartiesDao> InvolvedParties { get; } = new List<InvolvedPartiesDao>();

        /// <summary>
        /// Gets the collection of counter proposals related to the proposal.
        /// </summary>
        public ICollection<CounterProposalDao> CounterProposals { get; } = new List<CounterProposalDao>();
    }

}
