namespace Assigment.Models
{
    public class ItemParty
    {
        public Guid ItemId { get; set; } 
        public Guid PartyId { get; set; } 

        public Item Item { get; set; } = null!; 
        public Party Party { get; set; } = null!; 
    }
}
