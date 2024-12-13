using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.Models
{
    public class Proposal
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; } 
        public int ItemId { get; set; } 
        public int CreatedByUserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool? IsAccepted { get; set; } // Null = pending, true = accepted, false = rejected
        public DateTime CreatedDate { get; set; }

        public Item Item { get; set; } = null!;
        public User CreatedByUser { get; set; } = null!; 

        public ICollection<ProposalResponse> Responses { get; set; } = new List<ProposalResponse>(); 
    }
}
