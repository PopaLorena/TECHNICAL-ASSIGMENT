using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    public class InvolvedPartiesDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public bool? IsAccepted { get; set; }// Null = pending, true = accepted, false = rejected
        public Guid PartyId { get; set; }
        public Guid ProposalId { get; set; }
        public Guid CounterProposalId { get; set; }
        public Guid AcceptedByUserId { get; set; } 
    }
}
