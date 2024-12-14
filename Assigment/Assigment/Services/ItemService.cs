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

        public async Task<Item> AddItem(Item item)
        {
            item.CreatedDate = DateTime.UtcNow;
            List<Guid> partyIds = [];
            if (item.PartyIds.Count != 0)
            {
                foreach (var partyId in item.PartyIds)
                {
                    if (await partyRepository.FindParty(partyId).ConfigureAwait(false) == null)
                        throw new KeyNotFoundException("One of the Parties was not found.");
                    partyIds.Add(partyId);
                }
            }
            var newItemId = await itemRepository.AddItem(item).ConfigureAwait(false);
            if(partyIds.Count != 0)
                await itemRepository.AddItemToParty(partyIds, newItemId).ConfigureAwait(false);

            return await itemRepository.GetItemById(newItemId).ConfigureAwait(false);
        }

        public async Task<List<Item>> GetAllItemsFromMyParty(string userEmail)
        {
            var user = await userRepository.GetUserByEmail(userEmail).ConfigureAwait(false);

            return await itemRepository.GetAllItemsByPartyId(user.PartyId).ConfigureAwait(false);
        }

        public async Task<List<Item>> GetFilteredAndSortedItems(Item item, string userEmail)
        {
            if (item.SortBy is not ("Name" or "CreatedDate" or "IsShared"))
            {
                throw new ArgumentException($"Invalid value for SortBy. Allowed values are: 'Name', 'CreatedDate', 'IsShared'.");
            }
            if (item.SortOrder is not ("asc" or "desc"))
            {
                throw new ArgumentException($"Invalid value for SortOrder. Allowed values are: 'asc', 'desc'.");
            }

            var user = await userRepository.GetUserByEmail(userEmail).ConfigureAwait(false);

            var query = await itemRepository.GetAllItemsByPartyId(user.PartyId).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(item.Name))
            {
                query = query.Where(i => i.Name!.Contains(item.Name)).ToList();
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
            if (item.SortBy == "Name")
            {
                query = item.SortOrder == "asc"
                ? query.OrderBy(i => i.Name).ToList()
                    : query.OrderByDescending(i => i.Name).ToList();
            }
            else if (item.SortBy == "CreatedDate")
            {
                query = item.SortOrder == "asc"
                ? query.OrderBy(i => i.CreatedDate).ToList()
                    : query.OrderByDescending(i => i.CreatedDate).ToList();
            }
            else if (item.SortBy == "IsShared")
            {
                query = item.SortOrder == "asc"
                ? query.OrderBy(i => i.IsShared).ToList()
                    : query.OrderByDescending(i => i.IsShared).ToList();
            }

            return query;
        }
    }
}
