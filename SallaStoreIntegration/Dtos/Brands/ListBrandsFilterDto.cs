namespace SallaStoreIntegration.Dtos.Brands
{
    public class ListBrandsFilterDto
    {
        public string? Keyword { get; set; }
        public int? Page { get; set; }
        public string? With { get; set; }
    }
}
