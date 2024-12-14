using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    public interface IPartyService
    {
        Task<Party> AddParty(Party party);
    }
}
