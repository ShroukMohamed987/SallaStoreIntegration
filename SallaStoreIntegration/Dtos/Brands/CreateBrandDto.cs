using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Brands
{
    public class CreateBrandDto
    {
        [JsonProperty("name")]
        public string  Name { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
        [JsonProperty("banner")]
        public string banner { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
