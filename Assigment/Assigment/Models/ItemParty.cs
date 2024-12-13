using System.IO;

namespace Assigment.Models
{
    public class ItemParty
    {
        public int ItemId { get; set; } 
        public int PartyId { get; set; } 

        public Item Item { get; set; } = null!; 
        public Party Party { get; set; } = null!; 
    }
}
