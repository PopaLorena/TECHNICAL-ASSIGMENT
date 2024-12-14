using Assigment.Models;

namespace Assigment.ModelsDto.GetModels
{
    public class PartyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<InvolvedParties> InvolvedParties { get; set; } = [];
    }
}
