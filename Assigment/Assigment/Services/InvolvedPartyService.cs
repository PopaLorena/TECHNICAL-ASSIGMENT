using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;

namespace Assigment.Services
{
    /// <inheritdoc/>
    public class InvolvedPartyService : IInvolvedPartyService
    {
        private readonly IProposalRepository proposalRepository;
        private readonly ICounterProposalRepository counterProposalRepository;
        private readonly IUserRepository userRepository;
        private readonly IInvolvedPartyRepository involvedPartyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemService"/> class.
        /// </summary>
        /// <param name="itemRepository">The repository responsible for handling item data.</param>
        /// <param name="userRepository">The repository responsible for handling user data.</param>
        /// <param name="partyRepository">The repository responsible for handling party data.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters is <c>null</c>.</exception>
        public InvolvedPartyService(ICounterProposalRepository counterProposalRepository, IProposalRepository proposalRepository, IInvolvedPartyRepository involvedPartyRepository, IUserRepository userRepository)
        {
            this.counterProposalRepository = counterProposalRepository ?? throw new ArgumentNullException(nameof(counterProposalRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.involvedPartyRepository = involvedPartyRepository ?? throw new ArgumentNullException(nameof(involvedPartyRepository));
            this.proposalRepository = proposalRepository ?? throw new ArgumentNullException(nameof(proposalRepository));
        }

        /// <inheritdoc/>
        public async Task RespondToProposal(Guid proposalId, string userId, bool respons)
        {
            InvolvedParties involvedParty;
            var user = await userRepository.GetUserById(userId).ConfigureAwait(false);

            var proposal = await proposalRepository.GetProposalById(proposalId).ConfigureAwait(false);

            ValidateProposal(proposal, user.PartyId, respons);

            involvedParty = proposal.InvolvedParties.FirstOrDefault(ip => ip.PartyId == user!.PartyId);

            if (involvedParty == null)
            {
                throw new KeyNotFoundException("Your Party is not involved in this Proposal");
            }

            await involvedPartyRepository.RespontToProposal(involvedParty, user, respons).ConfigureAwait(false);
        }

        private void ValidateProposal(Proposal proposal, Guid partyId, bool respons)
        {
            if (proposal == null)
            {
                throw new KeyNotFoundException("the Proposal does not exist");
            }

            if (partyId == proposal.CreatedByUserId)
                if (respons)
                    throw new InvalidOperationException("Can’t accept my own proposal");
                else
                    throw new InvalidOperationException("Can’t reject my own proposal");
        }
    }
}
