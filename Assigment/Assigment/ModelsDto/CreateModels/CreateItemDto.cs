using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Item creation model
    /// </summary>
    public class CreateItemDto
    {
        /// <summary>
        /// The unique name associated with the item
        /// </summary>
        /// <example>name</example>
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        /// <summary>
        /// Indicates whether the item is shared with other parties.
        /// Default value: false
        /// </summary>
        /// <example>false</example>
        public bool IsShared { get; set; } = false;

        /// <summary>
        /// The list of party identifiers associated with the proposal.
        /// </summary>
        public List<Guid> PartyIds { get; set; } = [];
    }
}
