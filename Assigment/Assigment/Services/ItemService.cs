using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;

namespace Assigment.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository itemRepository;
        private readonly IUserRepository userRepository;
        private readonly IPartyRepository partyRepository;

        public ItemService(IItemRepository itemRepository, IUserRepository userRepository, IPartyRepository partyRepository)
        {
            this.partyRepository = partyRepository ?? throw new ArgumentNullException(nameof(partyRepository));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));
        }
        public async Task AddItem(Item item)
        {
            var newItemId = await itemRepository.AddItem(item).ConfigureAwait(false);
            item.CreatedDate = DateTime.UtcNow;

            if (item.IsShared == null) item.IsShared = false;

            if (item.PartyIds.Count != 0)
            {
                foreach (var partyId in item.PartyIds)
                {
                    if (await partyRepository.FindParty(partyId).ConfigureAwait(false) == null)
                        throw new KeyNotFoundException("One of the Parties was not found.");
                }
                await itemRepository.AddItemToParty(item.PartyIds, newItemId).ConfigureAwait(false);
            }
        }

        public async Task<List<Item>> GetAllItemsFromMyParty(string userEmail)
        {
            var userDao = await userRepository.GetUserByEmail(userEmail).ConfigureAwait(false);

            return await itemRepository.GetAllItemsByPartyId(userDao.PartyId).ConfigureAwait(false);
        }

        public async Task<List<Item>> GetFilteredAndSortedItems(Item item, string userEmail, string? sortBy, string? sortOrder)
        {
            var userDao = await userRepository.GetUserByEmail(userEmail).ConfigureAwait(false);

            var query = await itemRepository.GetAllItemsByPartyId(userDao.PartyId).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(item.Name))
            {
                query = query.Where(i => i.Name.Contains(item.Name)).ToList();
            }
            if (item.CreatedDate.HasValue)
            {
                query = query.Where(i => i.CreatedDate!.Value.Date == item.CreatedDate.Value.Date).ToList();
            }

            if (item.IsShared.HasValue)
            {
                query = query.Where(i => i.IsShared == item.IsShared).ToList();
            }

            // Apply sorting
            if (sortBy == "Name")
            {
                query = sortOrder == "asc"
                ? query.OrderBy(i => i.Name).ToList()
                    : query.OrderByDescending(i => i.Name).ToList();
            }
            else if (sortBy == "CreatedDate")
            {
                query = sortOrder == "asc"
                ? query.OrderBy(i => i.CreatedDate).ToList()
                    : query.OrderByDescending(i => i.CreatedDate).ToList();
            }
            else if (sortBy == "IsShared")
            {
                query = sortOrder == "asc"
                ? query.OrderBy(i => i.IsShared).ToList()
                    : query.OrderByDescending(i => i.IsShared).ToList();
            }

            return query;
        }
    }
}
