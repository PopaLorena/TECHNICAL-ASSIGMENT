using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    public interface IProposalRepository
    {
        Task<Proposal> AddProposal(Proposal proposal);
        Task<List<Proposal>> GetAllNegotiationDetails(Guid itemId);
    }
}
