using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    /// <inheritdoc/>
    public class PartyRepository : IPartyRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartyRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for interacting with the database.</param>
        /// <param name="mapper">The object mapper used to map between entities and DTOs.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="context"/> or <paramref name="mapper"/> is <c>null</c>.</exception>
        public PartyRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<Party> AddParty(Party party)
        {
            var partyDao = mapper.Map<PartyDao>(party);

            await context.Parties.AddAsync(partyDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return mapper.Map<Party>(partyDao);
        }

        /// <inheritdoc/>
        public async Task<Party?> FindParty(Guid partyId)
        {
            var partyDao = await context.Parties.FirstOrDefaultAsync(p => p.Id == partyId).ConfigureAwait(false);

            return mapper.Map<Party>(partyDao);
        }

        public async Task<List<Guid>> GetPartiesByItemIdAsync(Guid itemId)
        {
            return await context.ItemParties
                .Where(ip => ip.ItemId == itemId) 
                .Select(ip => ip.PartyId)        
                .ToListAsync();
        }
    }
}
