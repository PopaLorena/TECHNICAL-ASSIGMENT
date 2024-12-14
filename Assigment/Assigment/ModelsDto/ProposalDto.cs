namespace Assigment.ModelsDto
{
    public class ProposalDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public bool? IsAccepted { get; set; } // Null = pending, true = accepted, false = rejected
        public DateTime CreatedDate { get; set; }
        public string Payment { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }
    }

    public enum PaymentType
    {
        Percentages = 1,
        Amounts = 2
    }
}
