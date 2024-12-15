using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class UserDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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
        /// Gets or sets the password hash for the user (for authentication purposes).
        /// </summary>
        public required byte[] PasswordHash { get; set; }

        /// <summary>
        /// Gets or sets the password salt for the user (for password hashing).
        /// </summary>
        public required byte[] PasswordSalt { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the party that the user belongs to.
        /// </summary>
        public Guid PartyId { get; set; }

        /// <summary>
        /// Gets or sets the party that the user is a part of.
        /// </summary>
        public PartyDao Party { get; set; }

        /// <summary>
        /// Gets the collection of proposals created by the user.
        /// </summary>
        public ICollection<ProposalDao> Proposals { get; } = new List<ProposalDao>();

        /// <summary>
        /// Gets the collection of counter proposals created by the user.
        /// </summary>
        public ICollection<CounterProposalDao> CounterProposals { get; } = new List<CounterProposalDao>();

        /// <summary>
        /// Gets the collection of involved parties that the user is part of.
        /// </summary>
        public ICollection<InvolvedPartiesDao> InvolvedParties { get; } = new List<InvolvedPartiesDao>();
    }

}
