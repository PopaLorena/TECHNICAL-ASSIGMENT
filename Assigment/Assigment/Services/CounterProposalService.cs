using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using System.Globalization;

namespace Assigment.Services
{
    /// <inheritdoc/>
    public class CounterProposalService : ICounterProposalService
    {
        private readonly ICounterProposalRepository counterProposalRepository;
        private readonly IUserRepository userRepository;
        private readonly IInvolvedPartyRepository involvedPartyRepository;
        private readonly IProposalRepository proposalRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="CounterProposalService"/> class.
        /// </summary>
        /// <param name="counterProposalRepository">The repository responsible for handling counter proposal data.</param>
        /// <param name="userRepository">The repository responsible for handling user data.</param>
        /// <param name="involvedPartyRepository">The repository responsible for handling involved party data.</param>
        /// <param name="proposalRepository">The repository responsible for handling proposal data.</param>
        /// <exception cref="ArgumentNullException">Thrown when any of the parameters are <c>null</c>.</exception>
        public CounterProposalService(ICounterProposalRepository counterProposalRepository, IUserRepository userRepository, IInvolvedPartyRepository involvedPartyRepository, IProposalRepository proposalRepository)
        {
            this.counterProposalRepository = counterProposalRepository ?? throw new ArgumentNullException(nameof(counterProposalRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.involvedPartyRepository = involvedPartyRepository ?? throw new ArgumentNullException(nameof(involvedPartyRepository));
            this.proposalRepository = proposalRepository ?? throw new ArgumentNullException(nameof(proposalRepository));
        }

        /// <inheritdoc/>
        public async Task<CounterProposal> AddCounterProposal(CounterProposal counterProposal, string userId)
        {
            var proposal = await proposalRepository.GetProposalById(counterProposal.ProposalId).ConfigureAwait(false);
            var user = await userRepository.GetUserById(userId).ConfigureAwait(false);
            if (proposal == null)
            {
                throw new KeyNotFoundException("the Proposal does not exist");
            }

            if (user.PartyId == proposal.CreatedByUserId)
                throw new InvalidOperationException("Can’t create counter proposal to my own proposal");

            counterProposal.Payment = FormatPayment(counterProposal.PaymentType, counterProposal.Payment);

            counterProposal.CreatedDate = DateTime.Now;
            counterProposal.CreatedByUserId = user!.Id;

            counterProposal = await counterProposalRepository.AddCounterProposal(counterProposal).ConfigureAwait(false);

            return await counterProposalRepository.GetCounterProposalById(counterProposal.Id).ConfigureAwait(false);
        }

        private static bool IsValidPercentage(string input)
        {
            if (double.TryParse(input, out double number))
            {
                return number >= 0 && number <= 100;
            }
            return false;
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
    }
}
