using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;

namespace Assigment.Services
{
    /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<List<Item>> GetAllItemsFromMyParty(string userId)
        {
            var user = await userRepository.GetUserById(userId).ConfigureAwait(false);

            return await itemRepository.GetAllItemsByPartyId(user.PartyId).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<List<Item>> GetFilteredAndSortedItems(Item item, string userId)
        {
            ValidateSortInput(item.SortBy, item.SortOrder);

            var user = await userRepository.GetUserById(userId).ConfigureAwait(false);

            var query = await itemRepository.GetAllItemsByPartyId(user.PartyId).ConfigureAwait(false);

            query = ApplyFilter(query, item);

           
            query = ApplySorting(query, item);

            return query;
        }

        private static List<Item> ApplySorting(List<Item> query, Item item)
        {
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

        private static List<Item> ApplyFilter(List<Item> query, Item item)
        {
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

            return query;
        }

        private static void ValidateSortInput(string sortBy, string sortOrder)
        {
            if (sortBy is not ("Name" or "CreatedDate" or "IsShared"))
            {
                throw new ArgumentException($"Invalid value for SortBy. Allowed values are: 'Name', 'CreatedDate', 'IsShared'.");
            }
            if (sortOrder is not ("asc" or "desc"))
            {
                throw new ArgumentException($"Invalid value for SortOrder. Allowed values are: 'asc', 'desc'.");
            }
        }
    }
}
