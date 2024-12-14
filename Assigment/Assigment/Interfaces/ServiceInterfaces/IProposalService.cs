using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    public interface IProposalService
    {
        Task<Proposal> AddProposal(Proposal proposal, string userEmail);
        Task<List<Proposal>> GetAllNegotiationDetails(string userEmail, Guid itemId);
    }
}
