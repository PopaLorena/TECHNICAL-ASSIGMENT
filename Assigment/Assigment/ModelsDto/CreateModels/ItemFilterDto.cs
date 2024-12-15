namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for filtering items.
    /// </summary>
    public class ItemFilterDto
    {
        /// <summary>
        /// Gets or sets the name of the item to filter.
        /// </summary>
        /// <example>Sample Item</example>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the creation date of the item to filter.
        /// </summary>
        /// <example>2024-12-15T00:00:00</example>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is shared.
        /// </summary>
        /// <example>true</example>
        public bool? IsShared { get; set; }

        /// <summary>
        /// Gets or sets the field by which to sort the items.
        /// Possible values are "Name","CreatedDate" or "IsShared".
        /// Default value: Name
        /// </summary>
        /// <example>Name</example>
        public string SortBy { get; set; } = "Name";

        /// <summary>
        /// Gets or sets the sort order for the items.
        /// Possible values are "asc" or "desc".
        /// Default value: asc
        /// </summary>
        /// <example>asc</example>
        public string SortOrder { get; set; } = "asc";
    }
}
