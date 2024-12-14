using Assigment.Models;

namespace Assigment.Interfaces.ServiceInterfaces
{
    public interface IItemService
    {
        Task<Item> AddItem(Item item);
        Task<List<Item>> GetAllItemsFromMyParty(string userEmail);
        Task<List<Item>> GetFilteredAndSortedItems(Item item, string userEmail);
    }
}
