using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;

namespace Assigment.Services
{
    /// <inheritdoc/>
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository partyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyService"/> class.
        /// </summary>
        /// <param name="partyRepository">The repository responsible for handling party data.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="partyRepository"/> is <c>null</c>.</exception>
        public PartyService(IPartyRepository partyRepository)
        {
            this.partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
        }

        /// <inheritdoc/>
        public async Task<Party> AddParty(Party party)
        {
            return await partyRepository.AddParty(party).ConfigureAwait(false);
        }
    }
}
