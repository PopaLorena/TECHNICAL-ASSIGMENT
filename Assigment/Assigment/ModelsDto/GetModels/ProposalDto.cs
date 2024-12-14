using Assigment.Models;

namespace Assigment.ModelsDto.GetModels
{
    public class ProposalDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Payment { get; set; } = string.Empty;
        
        public List<InvolvedParties> InvolvedParties { get; set; } = [];
    }
}
