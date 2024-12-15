namespace Assigment.Models
{
    /// <summary>
    /// Represents an item that can be associated with one or more parties.
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the item. It is an optional field.
        /// </summary>
        public string? Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the item was created. It is an optional field.
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the item is shared. It is an optional field.
        /// </summary>
        public bool? IsShared { get; set; }

        /// <summary>
        /// Gets or sets the list of party identifiers associated with the item.
        /// </summary>
        public List<Guid> PartyIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Gets or sets the field by which the item should be sorted. It is an optional field.
        /// </summary>
        public string? SortBy { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the order in which the item should be sorted (e.g., ascending or descending). It is an optional field.
        /// </summary>
        public string? SortOrder { get; set; } = string.Empty;
    }

}

