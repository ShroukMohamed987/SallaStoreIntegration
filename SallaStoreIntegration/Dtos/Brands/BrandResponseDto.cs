using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Brands
{
    public class BrandResponseDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("status")]
        public bool? Status { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("banner")]
        public string? Banner { get; set; }

        [JsonProperty("logo")]
        public string? Logo { get; set; }

        [JsonProperty("ar_char")]
        public string? ArChar { get; set; }

        [JsonProperty("en_char")]
        public string? EnChar { get; set; }

        [JsonProperty("channels")]
        public List<string>? Channels { get; set; }

        [JsonProperty("metadata")]
        public object? Metadata { get; set; }
    }
}
