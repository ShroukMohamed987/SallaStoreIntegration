using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SallaStoreIntegration.Enums.Products;


namespace SallaStoreIntegration.Dtos.Product
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductStatusEnum? Status { get; set; }

        [JsonProperty("product_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ProductTypeEnum? product_type { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("categories")]
        public List<long>? Categories { get; set; }

        [JsonProperty("sale_price")]
        public decimal? sale_price { get; set; }

        [JsonProperty("cost_price")]
        public decimal? cost_price { get; set; }

        [JsonProperty("sku")]
        public string? SKU { get; set; }

        [JsonProperty("enable_upload_image")]
        [JsonIgnore]  // Newtonsoft's JsonIgnore
        public bool EnableUploadImage => true;

        [JsonProperty("brand_id")]
        public long? BrandId { get; set; }

        [JsonProperty("images")]
        public List<imageDto>? Images { get; set; }
    }
}
