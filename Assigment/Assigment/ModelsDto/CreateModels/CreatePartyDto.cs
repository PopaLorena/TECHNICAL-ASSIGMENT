using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    public class CreatePartyDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
