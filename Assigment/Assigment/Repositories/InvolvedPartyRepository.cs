using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;

namespace Assigment.Repositories
{
    /// <inheritdoc/>
    public class InvolvedPartyRepository : IInvolvedPartyRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvolvedPartyRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for interacting with the database.</param>
        /// <param name="mapper">The object mapper used to map between entities and DTOs.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="context"/> or <paramref name="mapper"/> is <c>null</c>.</exception>
        public InvolvedPartyRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task AddInvolvedParty(Guid partyId, Guid proposalId)
        {
            var involvedPartiesDao = new InvolvedPartiesDao { PartyId = partyId, ProposalId = proposalId };

            await context.InvolvedParties.AddAsync(involvedPartiesDao).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task RespontToProposal(InvolvedParties involvedParty, UserModel user, bool respons)
        {
            var involvedPartyDao = context.InvolvedParties.FirstOrDefault(p => p.Id == involvedParty.Id);

            if (involvedPartyDao != null)
            {
                var userDao = mapper.Map<UserDao>(user);
                involvedPartyDao!.AcceptedByUserId = user!.Id;

                involvedPartyDao.IsAccepted = respons;
                involvedPartyDao.AcceptedByUser = userDao;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
