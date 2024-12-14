namespace Assigment.Models
{
    public class InvolvedParties
    {
        public Guid Id { get; set; }
        public bool? IsAccepted { get; set; }// Null = pending, true = accepted, false = rejected
        public Guid PartyId { get; set; }
        public Guid ProposalId { get; set; }
        public Guid CounterProposalId { get; set; }

        public Guid AcceptedByUserId { get; set; }
        public string AcceptedByUser { get; set; } = string.Empty;
    }
}
