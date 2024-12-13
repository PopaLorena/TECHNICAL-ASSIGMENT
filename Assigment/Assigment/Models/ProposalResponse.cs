using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.Models
{
    public class ProposalResponse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; } 
        public int ProposalId { get; set; } 
        public int PartyId { get; set; } 
        public bool? IsAccepted { get; set; } // Null = pending, true = accepted, false = rejected

        public Proposal Proposal { get; set; } = null!; 
        public Party Party { get; set; } = null!; 
    }
}
