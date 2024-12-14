namespace Assigment.Models
{
    public class CounterProposal
    {
        public Guid Id { get; set; } 
        public Guid ProposalId { get; set; } 
        public Guid CreatedByUserId { get; set; } 
        public bool? IsAccepted { get; set; } // Null = pending, true = accepted, false = rejected

        public Proposal Proposal { get; set; } = null!; 
        public UserModel CreatedByUser { get; set; } = null!; 
    }
}
