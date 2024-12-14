using Assigment.Models;

namespace Assigment.Interfaces.RepositoryInterfaces
{
    public interface IItemRepository
    {
        Task<Guid> AddItem(Item item);
        Task AddItemToParty(List<Guid> partyIds, Guid id);
        Task<List<Item>> GetAllItemsByPartyId(Guid partyId);
        Task<Item> GetItemById(Guid newItemId);
    }
}
