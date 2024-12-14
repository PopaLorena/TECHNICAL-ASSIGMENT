namespace Assigment.ModelsDto
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool? IsShared { get; set; }
        public List<Guid> PartyIds { get; set; } = [];
    }
}
