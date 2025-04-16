using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Customers
{
    public class CustomerResponseDto
    {
        public int id { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
