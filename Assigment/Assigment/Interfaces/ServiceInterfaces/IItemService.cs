using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// Item-related services, including adding and retrieving items.
    /// </summary>
    public interface IItemService
    {
        /// <summary>
        /// Adds a new item to the system.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <returns>The added item on success.</returns>
        Task<Item> AddItem(Item item);

        /// <summary>
        /// Retrieves all items associated with the user's party.
        /// </summary>
        /// <param name="userId">The ID of the user to filter items by.</param>
        /// <returns>A list of items associated with the user's party.</returns>
        Task<List<Item>> GetAllItemsFromMyParty(string userId);

        /// <summary>
        /// Retrieves items filtered and sorted based on the provided parameters.
        /// </summary>
        /// <param name="item">The item criteria to filter by.</param>
        /// <param name="userId">The ID of the user to filter items by.</param>
        /// <returns>A list of filtered and sorted items.</returns>
        Task<List<Item>> GetFilteredAndSortedItems(Item item, string userId);
    }

}
