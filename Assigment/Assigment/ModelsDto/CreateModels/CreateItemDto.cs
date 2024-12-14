using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    public class CreateItemDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        public bool IsShared { get; set; } = false;

        public List<Guid> PartyIds { get; set; } = [];
    }
}
