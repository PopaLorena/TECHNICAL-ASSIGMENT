using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Migrations;
using Assigment.Models;
using System.Globalization;

namespace Assigment.Services
{
    public class ProposalService : IProposalService
    {
        private readonly IProposalRepository proposalRepository;
        private readonly IUserRepository userRepository;

        public ProposalService(IProposalRepository proposalRepository, IUserRepository userRepository)
        {
            this.proposalRepository = proposalRepository ?? throw new ArgumentNullException(nameof(proposalRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Proposal> AddProposal(Proposal proposal, string userEmail)
        {
            var user = await userRepository.GetUserByEmail(userEmail).ConfigureAwait(false);
            if(proposal.PaymentType == "Percentages")
            {
               var percentage = decimal.Parse(proposal.Payment);
               proposal.Payment = percentage.ToString("p0");
            }
            else if (proposal.PaymentType == "Amounts")
            {
                var cultureInfo = CultureInfo.GetCultureInfo("fr-FR");
                proposal.Payment = String.Format(cultureInfo, "{0:C} Euro", proposal.Payment);
            }
            else
            {
                throw new ArgumentException($"Invalid value for PaymentType. Allowed values are: 'Amounts', 'Percentages'.");
            }

            proposal.CreatedDate = DateTime.Now;
            proposal.CreatedByUserId = user!.Id;

            return await proposalRepository.AddProposal(proposal).ConfigureAwait(false);
        }

        public async Task<List<Proposal>> GetAllNegotiationDetails(string userEmail, Guid itemId)
        {
            var user = await userRepository.GetUserByEmail(userEmail).ConfigureAwait(false);

            return await proposalRepository.GetAllNegotiationDetails(itemId).ConfigureAwait(false);
        }
    }
}
