using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Assigment.ModelsDao
{
    public class PartyDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<UserDao> Users { get; } = [];
        [JsonIgnore]
        public ICollection<ItemPartyDao> ItemParties { get; } = [];
    }
}
