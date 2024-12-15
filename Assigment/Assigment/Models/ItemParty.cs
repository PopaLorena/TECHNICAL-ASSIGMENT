namespace Assigment.Models
{
    /// <summary>
    /// Represents the relationship between an item and a party. 
    /// </summary>
    public class ItemParty
    {
        /// <summary>
        /// Gets or sets the unique identifier of the associated item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated party.
        /// </summary>
        public Guid PartyId { get; set; }

        /// <summary>
        /// Gets or sets the associated item for the relationship.
        /// </summary>
        public Item Item { get; set; } = null!;

        /// <summary>
        /// Gets or sets the associated party for the relationship.
        /// </summary>
        public Party Party { get; set; } = null!;
    }

}
