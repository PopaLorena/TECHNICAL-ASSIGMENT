using Assigment.DatabaseContext;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Repositories
{
    public class ItemRepository : IItemRepository
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ItemRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Guid> AddItem(Item item)
        {
            item.CreatedDate = DateTime.Now;
            var itemDao = mapper.Map<ItemDao>(item);
            await context.Items.AddAsync(itemDao).ConfigureAwait(false);
           
            await context.SaveChangesAsync().ConfigureAwait(false);

            return itemDao.Id;
        }

        public async Task<List<Item>> GetAllItemsByPartyId(Guid partyId)
        {
            var partyWithItems = await context.Parties.Where(p => p.Id == partyId)
            .Include(p => p.ItemParties)
            .ThenInclude(ip => ip.Item)
            .FirstOrDefaultAsync();

            if (partyWithItems == null)
            {
                throw new KeyNotFoundException("The party not found.");
            }

            var itemsDao = partyWithItems.ItemParties.Select(ip => ip.Item).ToList();

            return mapper.Map<List<Item>>(itemsDao);
        }

        public async Task<Item> GetItemById(Guid newItemId)
        {
            var itemsDao = await context.Items.FirstOrDefaultAsync(i => i.Id == newItemId).ConfigureAwait(false);
            return mapper.Map<Item>(itemsDao);
        }

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
