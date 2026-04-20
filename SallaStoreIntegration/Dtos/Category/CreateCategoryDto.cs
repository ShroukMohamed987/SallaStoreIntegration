using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SallaStoreIntegration.Dtos.Category
{
    public class CreateCategoryDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
