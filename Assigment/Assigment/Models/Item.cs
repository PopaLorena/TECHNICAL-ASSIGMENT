using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.Models
{
    public class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsShared { get; set; }

        public ICollection<ItemParty> ItemParties { get; set; } = new List<ItemParty>(); 
        public ICollection<Proposal> Proposals { get; set; } = new List<Proposal>(); 
    }
}

