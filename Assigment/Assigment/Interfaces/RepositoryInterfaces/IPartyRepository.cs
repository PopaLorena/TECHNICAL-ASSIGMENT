using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    /// <summary>
    /// Handling party data interactions with the repository.
    /// </summary>
    public interface IPartyRepository
    {
        /// <summary>
        /// Adds a new party to the repository.
        /// </summary>
        /// <param name="party">The party to be added.</param>
        /// <returns>The added party as the result.</returns>
        Task<Party> AddParty(Party party);

        /// <summary>
        /// Finds a party by its ID.
        /// </summary>
        /// <param name="partyId">The ID of the party to find.</param>
        /// <returns>The found party or null if not found.</returns>
        Task<Party?> FindParty(Guid partyId);

        /// <summary>
        /// Retrieves a list of party IDs associated with a specific item.
        /// </summary>
        /// <param name="itemId">The ID of the item for which to retrieve associated parties.</param>
        /// <returns>A list of party IDs.</returns>
        Task<List<Guid>> GetPartiesByItemIdAsync(Guid itemId);
    }

}
