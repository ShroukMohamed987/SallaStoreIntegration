namespace ExternalStores.DTO.Salla
{
    public class ProductResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Message { get; set; }

    }

    public class FilterProductDto
    {
        public int? page { get; set; }
        public string? keyword { get; set; }
        public string? status { get; set; }
        public long? category { get; set; }
        public int? per_page { get; set; }
        public string Token { get; set; } = null!;
    }
}
