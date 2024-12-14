using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    public interface IPartyRepository
    {
        Task<Party> AddParty(Party party);

        Task<Party?> FindParty(Guid partyId);
    }
}
