
namespace Assigment.ModelsDto.GetModels
{
    public class InvolvedPartiesDto
    {
        public Guid Id { get; set; }
        public bool? IsAccepted { get; set; }// Null = pending, true = accepted, false = rejected
        public Guid PartyId { get; set; }
        public string AcceptedByUser { get; set; } = string.Empty; 
    }
}
