using Newtonsoft.Json;

namespace SallaStoreIntegration.Dtos.Customers
{
    public class UpdateCustomerDto
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

    }
}
