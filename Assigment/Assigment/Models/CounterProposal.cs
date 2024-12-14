namespace Assigment.Models
{
    public class CounterProposal
    {
        public Guid Id { get; set; } 
        public Guid ProposalId { get; set; } 
        public Guid CreatedByUserId { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;

        public List<InvolvedParties> InvolvedParties { get; set; } = [];
    }
}
