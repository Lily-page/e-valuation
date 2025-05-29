namespace PropertyAPI.Models
{
    public class PropertySearchRequest
    {
        public string? Region { get; set; }
        public string? District { get; set; }
        public string? PropertyType { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}