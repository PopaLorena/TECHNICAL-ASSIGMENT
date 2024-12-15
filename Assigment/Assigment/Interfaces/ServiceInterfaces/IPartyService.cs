using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// Party-related services, including adding new parties.
    /// </summary>
    public interface IPartyService
    {
        /// <summary>
        /// Adds a new party to the system.
        /// </summary>
        /// <param name="party">The party to be added.</param>
        /// <returns>The added party on success.</returns>
        Task<Party> AddParty(Party party);
    }

}
