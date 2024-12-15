using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Party creation model
    /// </summary>
    public class CreatePartyDto
    {
        /// <summary>
        /// The unique name associated with the item
        /// </summary>
        /// <example>Partyname</example>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
    }
}
