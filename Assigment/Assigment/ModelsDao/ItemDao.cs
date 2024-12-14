using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    public class ItemDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsShared { get; set; }

        
        public ICollection<ItemPartyDao> ItemParties { get; } = [];
        public ICollection<ProposalDao> Proposals { get; } = [];
    }
}

