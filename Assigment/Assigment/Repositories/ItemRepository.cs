using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    /// <inheritdoc/>
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for interacting with the database.</param>
        /// <param name="mapper">The object mapper used to map between entities and DTOs.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="context"/> or <paramref name="mapper"/> is <c>null</c>.</exception>
        public ItemRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <inheritdoc/>
        public async Task<Guid> AddItem(Item item)
        {
            item.CreatedDate = DateTime.Now;
            var itemDao = mapper.Map<ItemDao>(item);
            await context.Items.AddAsync(itemDao).ConfigureAwait(false);
           
            await context.SaveChangesAsync().ConfigureAwait(false);

            return itemDao.Id;
        }

        /// <inheritdoc/>
        public async Task<List<Item>> GetAllItemsByPartyId(Guid partyId)
        {
            var items = await context.Items
                .Where(item => item.ItemParties.Any(ip => ip.PartyId == partyId))
                .Include(item => item.ItemParties)  
                .ThenInclude(ip => ip.Party)   
                .ToListAsync();

            return mapper.Map<List<Item>>(items);
        }

        /// <inheritdoc/>
        public async Task<Item> GetItemById(Guid newItemId)
        {
            var itemsDao = await context.Items.FirstOrDefaultAsync(i => i.Id == newItemId).ConfigureAwait(false);
            return mapper.Map<Item>(itemsDao);
        }

        /// <inheritdoc/>
        public async Task AddItemToParty(List<Guid> partyIds, Guid newItemId)
        {
            foreach (var partyId in partyIds)
            {
                await context.ItemParties.AddAsync(new ItemPartyDao
                {
                    PartyId = partyId,
                    ItemId = newItemId
                }).ConfigureAwait(false);
            }
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
