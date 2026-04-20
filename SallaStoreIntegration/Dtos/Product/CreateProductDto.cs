
//using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using SallaStoreIntegration.Enums.Products;
using System.Text.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Product
{
    public class CreateProductDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("status")]
        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductStatusEnum Status { get; set; }

        [JsonPropertyName("product_type")]
        [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        public ProductTypeEnum product_type { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("categories")]
        public List<long> Categories { get; set; }

        [JsonPropertyName("sale_price")]
        public decimal sale_price { get; set; }

        [JsonPropertyName("cost_price")]
        public decimal cost_price { get; set; }

        [JsonPropertyName("sku")]
        public string SKU { get; set; }

        [JsonPropertyName("enable_upload_image")]
        [System.Text.Json.Serialization.JsonIgnore]
        public bool EnableUploadImage => true;

        [JsonPropertyName("brand_id")]
        public long? BrandId { get; set; }
        [JsonPropertyName("images")]
        public List<imageDto>? Images { get; set; }

    }
    public class imageDto
    {
        [JsonProperty("original")]
        public string Original { get; set; }

        [JsonProperty("thumbnail")]
        public string? Thumbnail { get; set; }
        [JsonProperty("alt")]
        public string? Alt { get; set; }
        [JsonProperty("default")]
        public bool? Default { get; set; }
        [JsonProperty("sort")]
        public int? Sort { get; set; }
    }



}
