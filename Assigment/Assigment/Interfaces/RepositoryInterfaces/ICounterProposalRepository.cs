using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    /// <summary>
    /// Hndling counter proposal data interactions with the repository.
    /// </summary>
    public interface ICounterProposalRepository
    {
        /// <summary>
        /// Adds a counter proposal to the repository.
        /// </summary>
        /// <param name="counterProposal">The counter proposal to be added.</param>
        /// <returns>The added counter proposal.</returns>
        Task<CounterProposal> AddCounterProposal(CounterProposal counterProposal);

        /// <summary>
        /// Retrieves a counter proposal by its ID.
        /// </summary>
        /// <param name="id">The ID of the counter proposal to be retrieved.</param>
        /// <returns>The counter proposal or null if not found.</returns>
        Task<CounterProposal> GetCounterProposalById(Guid id);
    }

}
