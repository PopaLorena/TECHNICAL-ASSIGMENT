using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    public interface IPartyService
    {
        Task AddParty(Party party);
    }
}
