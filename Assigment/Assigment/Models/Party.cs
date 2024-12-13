using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.Models
{
    public class Party
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>(); 
        public ICollection<ItemParty> Items { get; set; } = new List<ItemParty>(); 
    }
}
