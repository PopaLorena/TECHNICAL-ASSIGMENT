using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    /// <summary>
    /// Handling item data interactions with the repository.
    /// </summary>
    public interface IItemRepository
    {
        /// <summary>
        /// Adds a new item to the repository and returns the ID of the added item.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>The ID of the added item as the result.</returns>
        Task<Guid> AddItem(Item item);

        /// <summary>
        /// Associates an item with one or more parties by adding the item to the specified parties.
        /// </summary>
        /// <param name="partyIds">The list of party IDs to associate with the item.</param>
        /// <param name="id">The ID of the item to associate with the parties.</param>
        Task AddItemToParty(List<Guid> partyIds, Guid id);

        /// <summary>
        /// Retrieves all items associated with a specific party ID.
        /// </summary>
        /// <param name="partyId">The ID of the party for which to retrieve associated items.</param>
        /// <returns>A list of items associated with the specified party.</returns>
        Task<List<Item>> GetAllItemsByPartyId(Guid partyId);

        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        /// <param name="newItemId">The ID of the item to retrieve.</param>
        /// <returns>The item retrieved by ID.</returns>
        Task<Item> GetItemById(Guid newItemId);
    }

}
