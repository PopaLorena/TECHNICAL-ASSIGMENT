using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assigment.ModelsDao
{
    public class ItemPartyDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; } 
        public Guid PartyId { get; set; }

        [JsonIgnore]
        public ItemDao? Item { get; set; }
    }
}
