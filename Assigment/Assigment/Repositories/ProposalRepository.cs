using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    /// <inheritdoc/>
    public class ProposalRepository : IProposalRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProposalRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for interacting with the database.</param>
        /// <param name="mapper">The object mapper used to map between entities and DTOs.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="context"/> or <paramref name="mapper"/> is <c>null</c>.</exception>
        public ProposalRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<Proposal> AddProposal(Proposal proposal)
        {
            var proposalDao = mapper.Map<ProposalDao>(proposal);

            await context.Proposals.AddAsync(proposalDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return mapper.Map<Proposal>(proposalDao);
        }

        /// <inheritdoc/>
        public async Task<List<Proposal>> GetAllNegotiationDetails(Guid itemId)
        {
            var proposals = await context.Proposals
                .Where(p => p.ItemId == itemId) 
                .Include(p => p.InvolvedParties) 
                .Include(p => p.CounterProposals) 
                .ToListAsync().ConfigureAwait(false);

            return mapper.Map<List<Proposal>>(proposals); 
        }

        /// <inheritdoc/>
        public async Task<Proposal> GetProposalById(Guid id)
        {
            var proposalDao = await context.Proposals.Include(p=> p.InvolvedParties).FirstOrDefaultAsync(p=> p.Id == id).ConfigureAwait(false);
            return mapper.Map<Proposal>(proposalDao);
        }
    }
}
