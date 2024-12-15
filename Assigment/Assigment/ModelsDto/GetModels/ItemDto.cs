namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for an item.
    /// </summary>
    public class ItemDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the item.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is shared with others.
        /// </summary>
        public bool? IsShared { get; set; }

        /// <summary>
        /// Gets or sets the list of party identifiers associated with the item.
        /// </summary>
        public List<Guid> PartyIds { get; set; } = [];
    }
}
