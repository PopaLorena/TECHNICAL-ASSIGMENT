using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// Proposal-related services, including adding proposals and retrieving negotiation details.
    /// </summary>
    public interface IProposalService
    {
        /// <summary>
        /// Adds a new proposal to the system.
        /// </summary>
        /// <param name="proposal">The proposal to be added.</param>
        /// <param name="userId">The ID of the user creating the proposal.</param>
        /// <returns>hTe added proposal on success.</returns>
        Task<Proposal> AddProposal(Proposal proposal, string userId);

        /// <summary>
        /// Retrieves all negotiation details for a specific item and user.
        /// </summary>
        /// <param name="userId">The ID of the user requesting the negotiation details.</param>
        /// <param name="itemId">The ID of the item associated with the negotiations.</param>
        /// <returns>A list of proposals associated with the item.</returns>
        Task<List<Proposal>> GetAllNegotiationDetails(string userId, Guid itemId);
    }

}
