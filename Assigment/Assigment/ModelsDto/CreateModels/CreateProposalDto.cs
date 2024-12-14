namespace Assigment.ModelsDto.CreateModels
{
    public class CreateProposalDto
    {
        public Guid ItemId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Payment { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;
    }
}
