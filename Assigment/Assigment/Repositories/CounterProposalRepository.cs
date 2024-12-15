using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    /// <inheritdoc/>
    public class CounterProposalRepository : ICounterProposalRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterProposalRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to interact with the underlying database.</param>
        /// <param name="mapper">The object mapper used to map between entities and DTOs.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> or <paramref name="mapper"/> is <c>null</c>.</exception>
        public CounterProposalRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<CounterProposal> AddCounterProposal(CounterProposal counterProposal)
        {
            var counterProposalDao = mapper.Map<CounterProposalDao>(counterProposal);

            await context.CounterProposal.AddAsync(counterProposalDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return mapper.Map<CounterProposal>(counterProposalDao);
        }

        /// <inheritdoc/>
        public async Task<CounterProposal> GetCounterProposalById(Guid id)
        {
            var counterProposalDao = await context.CounterProposal.FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
            return mapper.Map<CounterProposal>(counterProposalDao);
        }
    }
}
