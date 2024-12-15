namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for linking an item with a party.
    /// </summary>
    public class ItemPartyDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the party.
        /// </summary>
        public Guid PartyId { get; set; }

        /// <summary>
        /// Gets or sets the item associated with the party.
        /// </summary>
        public ItemDto Item { get; set; } = null!;

        /// <summary>
        /// Gets or sets the party associated with the item.
        /// </summary>
        public PartyDto Party { get; set; } = null!;
    }
}
