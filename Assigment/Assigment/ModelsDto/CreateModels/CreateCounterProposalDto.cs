namespace Assigment.ModelsDto.CreateModels
{
    public class CreateCounterProposalDto
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Payment { get; set; } = string.Empty;
    }
}
