namespace Assigment.Models
{
    /// <summary>
    /// Represents a party, which can be associated with multiple involved parties in various proposals.
    /// </summary>
    public class Party
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
        /// Gets or sets the list of involved parties associated with this party in proposals.
        /// </summary>
        public List<InvolvedParties> InvolvedParties { get; set; } = [];
    }

}
