using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Category
{
    public class CreateCategoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CategoryNodeDto
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("parent_id")]
        public long? ParentId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sub_categories")]
        [System.Text.Json.Serialization.JsonPropertyName("sub_categories")]
        public List<CategoryNodeDto>? Subcategories { get; set; } = new();

        public string? Message { get; set; }
    }
}
