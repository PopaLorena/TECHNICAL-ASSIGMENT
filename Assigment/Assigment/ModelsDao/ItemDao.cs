using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents an item in the system.
    /// </summary>
    public class ItemDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when the item was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is shared.
        /// </summary>
        public bool IsShared { get; set; }

        /// <summary>
        /// Gets the collection of parties associated with the item.
        /// </summary>
        public ICollection<ItemPartyDao> ItemParties { get; } = new List<ItemPartyDao>();

        /// <summary>
        /// Gets the collection of proposals associated with the item.
        /// </summary>
        public ICollection<ProposalDao> Proposals { get; } = new List<ProposalDao>();
    }

}

