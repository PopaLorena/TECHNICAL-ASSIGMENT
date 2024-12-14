using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public PartyRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddParty(Party party)
        {
            var partyDao = mapper.Map<PartyDao>(party);
            await context.Parties.AddAsync(partyDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Party> FindParty(Guid partyId)
        {
            var partyDao = await context.Parties.FirstOrDefaultAsync(p => p.Id == partyId).ConfigureAwait(false);

            return mapper.Map<Party>(partyDao);
        }
    }
}
