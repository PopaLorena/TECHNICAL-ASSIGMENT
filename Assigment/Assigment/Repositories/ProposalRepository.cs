using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ProposalRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Proposal> AddProposal(Proposal proposal)
        {
            var proposalDao = mapper.Map<ProposalDao>(proposal);

            await context.Proposals.AddAsync(proposalDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            return mapper.Map<Proposal>(proposalDao);
        }

        public async Task<List<Proposal>> GetAllNegotiationDetails(Guid itemId)
        {
            var itemDetails = await context.Items
                .Where(i => i.Id == itemId && i.IsShared)
                .Include(i => i.Proposals)
                    .ThenInclude(p => p.CounterProposals)
                .ThenInclude(cp => cp.InvolvedParties)
                .OrderBy(i => i.CreatedDate)
                .FirstOrDefaultAsync();

            if (itemDetails != null)
            {
                var proposalsWithCounterProposals = itemDetails.Proposals
                    .OrderBy(p => p.CreatedDate)
                    .Select(p => new
                    {
                        Proposal = p,
                        CounterProposals = p.CounterProposals.OrderBy(cp => cp.CreatedDate).ToList()
                    })
                    .ToList();
            }
            return mapper.Map<List<Proposal>>(itemDetails?.Proposals); 
        }
    }
}
