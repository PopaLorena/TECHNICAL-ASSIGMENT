namespace Assigment.ModelsDto.CreateModels
{
    public class ItemFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public bool? IsShared { get; set; }
        public string? SortBy { get; set; } = "Name";
        public string? SortOrder { get; set; } = "asc";
    }
}
