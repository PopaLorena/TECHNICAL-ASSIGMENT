using Assigment.Models;
using Assigment.ModelsDto;

namespace Assigment.Interfaces.ServiceInterfaces
{
    public interface IItemService
    {
        Task AddItem(Item item);
        Task<List<Item>> GetAllItemsFromMyParty(string userEmail);
        Task<List<Item>> GetFilteredAndSortedItems(Item item, string userEmail, string? SortBy, string? SortOrder);
    }
}
