using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// Handling counter proposals in the system.
    /// </summary>
    public interface ICounterProposalService
    {
        /// <summary>
        /// Allows a user to add a counter proposal.
        /// </summary>
        /// <param name="counterProposal">The counter proposal to be added.</param>
        /// <param name="userId">The ID of the user creating the counter proposal.</param>
        /// <returns>The added counter proposal as the result.</returns>
        Task<CounterProposal> AddCounterProposal(CounterProposal counterProposal, string userId);
    }

}
