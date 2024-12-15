using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Proposal creation model
    /// </summary>
    public class CreateProposalDto
    {
        /// <summary>
        /// the id of the item for which we are creating the proposal
        /// </summary>
        [Required(ErrorMessage = "ItemId is required")]
        public Guid ItemId { get; set; }

        /// <summary>
        /// Comment provided by the user
        /// </summary>
        /// <example>This is my feedback regarding the proposal.</example>
        public string Comment { get; set; } = string.Empty;

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
        public string PaymentType { get; set; } = string.Empty;
    }
}
