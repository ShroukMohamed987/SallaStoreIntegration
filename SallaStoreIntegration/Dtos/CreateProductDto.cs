using Newtonsoft.Json;

namespace ExternalStores.DTO.Salla
{
    public class CreateProductDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("product_type")]
        public string ProductType { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("categories")]
        public List<long> Categories { get; set; }

        [JsonProperty("sale_price")]
        public decimal SalePrice { get; set; }

        [JsonProperty("cost_price")]
        public decimal CostPrice { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("enable_upload_image")]
        [JsonIgnore]
        public bool EnableUploadImage => true;

        [JsonProperty("brand_id")]
        public long BrandId { get; set; }

    }
}
