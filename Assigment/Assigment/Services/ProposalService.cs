using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using System.Globalization;

namespace Assigment.Services
{
    /// <inheritdoc/>
    public class ProposalService : IProposalService
    {
        private readonly IProposalRepository proposalRepository;
        private readonly IUserRepository userRepository;
        private readonly IInvolvedPartyRepository involvedPartyRepository;
        private readonly IPartyRepository partyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProposalService"/> class.
        /// </summary>
        /// <param name="proposalRepository">The repository responsible for handling proposal data.</param>
        /// <param name="userRepository">The repository responsible for handling user data.</param>
        /// <param name="involvedPartyRepository">The repository responsible for handling involved party data.</param>
        /// <param name="partyRepository">The repository responsible for handling party data.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are <c>null</c>.</exception>
        public ProposalService(IProposalRepository proposalRepository, IUserRepository userRepository, IInvolvedPartyRepository involvedPartyRepository, IPartyRepository partyRepository)
        {
            this.proposalRepository = proposalRepository ?? throw new ArgumentNullException(nameof(proposalRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.involvedPartyRepository = involvedPartyRepository ?? throw new ArgumentNullException(nameof(involvedPartyRepository));
            this.partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
        }

        /// <inheritdoc/>
        public async Task<Proposal> AddProposal(Proposal proposal, string userId)
        {
            proposal.Payment = FormatPayment(proposal.PaymentType, proposal.Payment);

            var user = await userRepository.GetUserById(userId).ConfigureAwait(false);
            proposal.CreatedDate = DateTime.Now;
            proposal.CreatedByUserId = user!.Id;

            proposal = await proposalRepository.AddProposal(proposal).ConfigureAwait(false);

            var partiesIds = await partyRepository.GetPartiesByItemIdAsync(proposal.ItemId).ConfigureAwait(false);

            foreach (var partyId in partiesIds) 
            {
                await involvedPartyRepository.AddInvolvedParty(partyId, proposal.Id).ConfigureAwait(false);
            }

            return await proposalRepository.GetProposalById(proposal.Id).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<Proposal>> GetAllNegotiationDetails(string userId, Guid itemId)
        {
            var user = await userRepository.GetUserById(userId).ConfigureAwait(false);

            var proposals = await proposalRepository.GetAllNegotiationDetails(itemId).ConfigureAwait(false);

            foreach (Proposal proposal in proposals) {

                var createdByUser = await userRepository.GetUserById(proposal.CreatedByUserId.ToString()).ConfigureAwait(false);
                proposal.CreatedByUserName = createdByUser.Party.Id != user!.PartyId ? createdByUser.Party.Name : createdByUser.Name;

                foreach (var involedParty in proposal.InvolvedParties) 
                {
                    if (involedParty.IsAccepted != null)
                    {
                        var acceptedByUser = await userRepository.GetUserById(involedParty.AcceptedByUserId.ToString()).ConfigureAwait(false);
                        involedParty.AcceptedByUserName = acceptedByUser.Party?.Id == user!.PartyId ? involedParty.AcceptedByUser.Name : acceptedByUser.Party!.Name;
                    }
                }

                if (proposal != null && proposal.CounterProposals.Any())
                {
                    var orderedCounterProposals = proposal.CounterProposals
                        .OrderBy(cp => cp.CreatedDate)
                        .ToList();

                    proposal.CounterProposals.Clear();
                    foreach (var counterProposal in orderedCounterProposals)
                    {
                        counterProposal.CreatedByUserName = counterProposal.CreatedByUser.PartyId != user.PartyId ? counterProposal.CreatedByUser.Party.Name : counterProposal.CreatedByUser.Name;
                        proposal.CounterProposals.Add(counterProposal);
                    }
                }
            }
            return proposals;
        }

        private static string FormatPayment(string paymentType, string payment)
        {
            if (paymentType == "Percentages")
            {
                if (!IsValidPercentage(payment))
                    throw new ArgumentException($"Invalid value for Payment.");

                return payment + "%";
            }
            if (paymentType == "Amounts")
            {
                if (!decimal.TryParse(payment, out decimal amount))
                    throw new ArgumentException($"Invalid value for Payment.");

                var cultureInfo = CultureInfo.GetCultureInfo("fr-FR");
                return String.Format(cultureInfo, "{0:C} Euro", payment);
            }
            else
            {
                throw new ArgumentException($"Invalid value for PaymentType. Allowed values are: 'Amounts', 'Percentages'.");
            }
        }

        private static bool IsValidPercentage(string input)
        {
            if (double.TryParse(input, out double number))
            {
                return number >= 0 && number <= 100;
            }
            return false;
        }
    }
}
