using Assigment.Models;

namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for a party.
    /// </summary>
    public class PartyDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the party.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the party.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the list of involved parties associated with this party.
        /// </summary>
        public List<InvolvedParties> InvolvedParties { get; set; } = [];
    }
}
