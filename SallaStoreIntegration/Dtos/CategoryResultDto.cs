using Newtonsoft.Json;

namespace ExternalStores.DTO.Salla
{
    public class CategoryResultDto
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string? Message { get; set; }
    }
}
