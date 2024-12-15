using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    /// <summary>
    /// Handling involved party data interactions with the repository.
    /// </summary>
    public interface IInvolvedPartyRepository
    {
        /// <summary>
        /// Adds an involved party to a proposal.
        /// </summary>
        /// <param name="partyId">The ID of the party to be added.</param>
        /// <param name="proposalId">The ID of the proposal to which the party is involved.</param>
        Task AddInvolvedParty(Guid partyId, Guid proposalId);

        /// <summary>
        /// Responds to a proposal by an involved party.
        /// </summary>
        /// <param name="involvedParty">The involved party response details.</param>
        /// <param name="user">The user making the response.</param>
        /// <param name="respons">The response status (true for accepted, false for rejected).</param>
        Task RespontToProposal(InvolvedParties involvedParty, UserModel user, bool respons);
    }
}
