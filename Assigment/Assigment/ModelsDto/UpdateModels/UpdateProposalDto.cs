using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.GetModels
{
    /// <summary>
    /// Data Transfer Object (DTO) for updating a proposal.
    /// </summary>
    public class UpdateProposalDto
    {
        /// <summary>
        /// Comment provided by the user
        /// </summary>
        /// <example>This is my feedback regarding the proposal.</example>
        [Required(ErrorMessage = "Comment when updating is required")]
        public string Comment { get; set; }

        /// <summary>
        /// Payment details associated with the proposal.
        /// </summary>
        /// <example>500.00</example>
        [Required(ErrorMessage = "ItemId is required")]
        public string Payment { get; set; }

        /// <summary>
        /// Type of payment. Allowed values are: 'Amounts', 'Percentages'.
        /// Default value: Amounts
        /// </summary>
        /// <example>Amounts</example>
        [Required(ErrorMessage = "ItemId is required")]
        public string PaymentType { get; set; }
    }
}
