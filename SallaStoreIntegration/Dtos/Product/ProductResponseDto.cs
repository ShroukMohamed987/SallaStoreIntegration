using SallaStoreIntegration.Enums.Products;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Product
{
    public class ProductResponseDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("type")]
    
        public string Type { get; set; }

        [JsonPropertyName("status")]
        
        public string Status { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("price")]
        public PriceDto Price { get; set; }

        [JsonPropertyName("sale_price")]
        public PriceDto SalePrice { get; set; }

        [JsonPropertyName("cost_price")]
        public string CostPrice { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class PriceDto
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}
