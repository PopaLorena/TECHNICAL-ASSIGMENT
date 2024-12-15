using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assigment.ModelsDao
{
    /// <summary>
    /// Represents the relationship between an item and a party in the system.
    /// </summary>
    public class ItemPartyDao
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item-party relationship.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the item associated with this relationship.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the party associated with this relationship.
        /// </summary>
        public Guid PartyId { get; set; }

        /// <summary>
        /// Gets or sets the item associated with this relationship. 
        /// This property is ignored during JSON serialization.
        /// </summary>
        [JsonIgnore]
        public ItemDao? Item { get; set; }

        /// <summary>
        /// Gets or sets the party associated with this relationship.
        /// </summary>
        public PartyDao? Party { get; set; }
    }

}
