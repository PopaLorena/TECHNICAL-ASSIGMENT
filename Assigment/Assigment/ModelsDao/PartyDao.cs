using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents a party in the system, which can have multiple users, items, and involved parties.
    /// </summary>
    public class PartyDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the party.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the party.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets the collection of users associated with the party.
        /// </summary>
        public ICollection<UserDao> Users { get; } = new List<UserDao>();

        /// <summary>
        /// Gets the collection of item-party relationships associated with the party.
        /// </summary>
        public ICollection<ItemPartyDao> ItemParties { get; } = new List<ItemPartyDao>();

        /// <summary>
        /// Gets the collection of involved parties associated with the party.
        /// </summary>
        public ICollection<InvolvedPartiesDao> InvolvedParties { get; } = new List<InvolvedPartiesDao>();
    }

}
