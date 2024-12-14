namespace Assigment.ModelsDto
{
    public class ItemPartyDto
    {
        public Guid ItemId { get; set; }
        public Guid PartyId { get; set; }

        public ItemDto Item { get; set; } = null!;
        public PartyDto Party { get; set; } = null!;
    }
}
