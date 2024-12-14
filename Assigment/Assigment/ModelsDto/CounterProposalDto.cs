namespace Assigment.ModelsDto
{
    public class CounterProposalDto
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public bool? IsAccepted { get; set; }
        public Guid CreatedByUserId { get; set; }
    }
}
