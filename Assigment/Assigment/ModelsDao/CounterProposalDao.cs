using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    public class CounterProposalDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; } 
        public Guid ProposalId { get; set; } 
        public Guid CreatedByUserId { get; set; } 
        public bool? IsAccepted { get; set; } // Null = pending, true = accepted, false = rejected
    }
}
