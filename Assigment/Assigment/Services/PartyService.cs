using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;

namespace Assigment.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository partyRepository;

        public PartyService(IPartyRepository partyRepository)
        {
            this.partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
        }

        public async Task AddParty(Party party)
        {
            await partyRepository.AddParty(party).ConfigureAwait(false);
        }
    }
}
