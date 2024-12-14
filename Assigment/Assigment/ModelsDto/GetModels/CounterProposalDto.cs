using Assigment.Models;

namespace Assigment.ModelsDto.GetModels
{
    public class CounterProposalDto
    {
        public Guid Id { get; set; }
        public Guid ProposalId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Payment { get; set; } = string.Empty;

        public List<InvolvedParties> InvolvedParties { get; set; } = [];
    }
}
