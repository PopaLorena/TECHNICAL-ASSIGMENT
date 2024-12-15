using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDto.CreateModels
{
    /// <summary>
    /// Counter Proposal creation model
    /// </summary>
    public class CreateCounterProposalDto
    {
        /// <summary>
        /// Unique identifier for the proposal
        /// </summary>
        [Required(ErrorMessage = "ProposalId is required")]
        public Guid ProposalId { get; set; }

        /// <summary>
        /// Comment provided by the user
        /// </summary>
        /// <example>This is my feedback regarding the proposal.</example>
        [Required(ErrorMessage = "Name is required")]
        public string Comment { get; set; }

        /// <summary>
        /// Payment details associated with the proposal.
        /// </summary>
        /// <example>500.00</example>
        [Required(ErrorMessage = "Payment is required")]
        public string Payment { get; set; }


        /// <summary>
        /// Type of payment. Allowed values are: 'Amounts', 'Percentages'.
        /// Default value: Amounts
        /// </summary>
        /// <example>Amounts</example>
        [Required(ErrorMessage = "PaymentType is required")]
        public string PaymentType { get; set; } = "Amounts";
    }
}
