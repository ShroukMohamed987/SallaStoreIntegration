using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExternalStores.DTO.Salla
{
    public class CreateCategoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
