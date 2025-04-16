using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos
{
    public class CategoryUpdateResultDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string? Message { get; set; }
    }
}

