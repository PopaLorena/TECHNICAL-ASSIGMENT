namespace Assigment.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool? IsShared { get; set; }
        public List<Guid> PartyIds { get; set; } = [];
        public string? SortBy { get; set; } = string.Empty;
        public string? SortOrder { get; set; } = string.Empty;
    }
}

