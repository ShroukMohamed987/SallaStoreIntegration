using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Product
{
    public class BulkUpdateProductPriceRequestDto
    {
        [JsonProperty("products")]
        public List<BulkUpdateProductPriceDto> Products { get; set; }
    }
    public class BulkUpdateProductPriceDto
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("price")]
        public int price { get; set; }
        [JsonProperty("sale_price")]
        public decimal? sale_price { get; set; }

        [JsonProperty("cost_price")]
        public decimal? cost_price { get; set; }
        [JsonProperty("sale_end")]
        public string sale_end { get; set; }
    }
}
