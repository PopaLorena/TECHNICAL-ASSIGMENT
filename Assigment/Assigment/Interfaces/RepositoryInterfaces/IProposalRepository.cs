using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    /// <summary>
    /// Handling proposal data interactions with the repository.
    /// </summary>
    public interface IProposalRepository
    {
        /// <summary>
        /// Adds a new proposal to the repository.
        /// </summary>
        /// <param name="proposal">The proposal to be added.</param>
        /// <returns>The added proposal as the result.</returns>
        Task<Proposal> AddProposal(Proposal proposal);

        /// <summary>
        /// Retrieves all negotiation details for a specific item.
        /// </summary>
        /// <param name="itemId">The ID of the item to retrieve negotiations for.</param>
        /// <returns>A a list of proposals associated with the item.</returns>
        Task<List<Proposal>> GetAllNegotiationDetails(Guid itemId);

        /// <summary>
        /// Retrieves a proposal by its ID.
        /// </summary>
        /// <param name="id">The ID of the proposal to retrieve.</param>
        /// <returns>The proposal as the result.</returns>
        Task<Proposal> GetProposalById(Guid id);
    }

}
